using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Act_Season_RazeKudzu : Assets.Code.Action
    {
        public static int damageToArmy = 7;
        public static double damageToKudzu = 100;


        public Act_Season_RazeKudzu(Location loc) : base(loc)
        {
        }

        public override string getName()
        {
            return "Raze Kudzu";
        }

        public override string getShortDesc()
        {
            return "Aware rulers and those who dislike Madness can order their army to reduce Dreaming Kudzu in their territory or a neighbour by " + damageToKudzu + "%, causing the army to take " + damageToArmy + " damage in the process.";
        }

        public override Sprite getIconFore()
        {
            return map.world.iconStore.raze;
        }

        public override int getTurnsRequired()
        {
            return 3;
        }

        public override bool valid(Person ruler, SettlementHuman settlementHuman)
        {
            if (settlementHuman.supportedMilitary == null || map.units.Contains(settlementHuman.supportedMilitary) == false)
                return false;

            if (settlementHuman.supportedMilitary.task == null || settlementHuman.supportedMilitary.task is Task_Recruit)
            {
                if (ruler.awareness >= 1)
                    return true;

                if (ruler.getTagRanking(Tags.MADNESS) <= -1)
                    return true;
            }


            return false;
        }

        public override double getUtility(SettlementHuman hum, Person ruler, List<ReasonMsg> reasons)
        {
            double utility = base.getUtility(hum, ruler, reasons);

            utility -= 50;
            reasons?.Add(new ReasonMsg("Base Reluctance", -50));

            if (hum.supportedMilitary != null && hum.supportedMilitary.hp <= damageToArmy)
            {
                utility -= 60;
                reasons?.Add(new ReasonMsg("Would Destroy Army", -60));
            }

            foreach (Property pr in location.properties)
            {
                if (pr is Pr_Season_DreamingKudzu)
                {
                    utility += pr.charge / 4;
                    reasons?.Add(new ReasonMsg("Kudzu in Home", pr.charge / 4));
                }
            }

            double neighbouringKudzu = 0;
            double foreignerKudzu = 0;
            foreach (Location l in location.getNeighbours())
            {
                foreach (Property pr in l.properties)
                {
                    if (pr is Pr_Season_DreamingKudzu)
                    {
                        if (l.soc == location.soc || l.soc == null)
                            neighbouringKudzu += pr.charge;
                        else
                            foreignerKudzu += pr.charge;
                    }
                }
            }

            if (neighbouringKudzu > 0)
            {
                utility += neighbouringKudzu / 4;
                reasons?.Add(new ReasonMsg("Kudzu in Neighbours", neighbouringKudzu / 4));
            }
            if (foreignerKudzu > 0)
            {
                utility += neighbouringKudzu / 8;
                reasons?.Add(new ReasonMsg("Kudzu in Foreign Neighbours", neighbouringKudzu / 8));
            }

            return utility;
        }

        public override int[] getNegativeTags()
        {
            return new int[2] { Tags.MADNESS, map.soc_dark.index + 20000 };
        }

        public override void complete()
        {
            base.complete();

            if (location.settlement is SettlementHuman sh)
            {
                if (sh.supportedMilitary != null)
                {
                    Pr_Season_DreamingKudzu bestTarget = null;
                    double bestValue = 0;

                    foreach (Property pr in location.properties)
                    {
                        if (pr is Pr_Season_DreamingKudzu kudzu)
                        {
                            bestTarget = kudzu;
                            bestValue = kudzu.charge * 2;
                        }
                    }
                    foreach (Location l in location.getNeighbours())
                    {
                        foreach (Property pr in location.properties)
                        {
                            if (pr is Pr_Season_DreamingKudzu kudzu)
                            {
                                if (l.soc == null || l.soc == location.soc)
                                {
                                    if (kudzu.charge > bestValue)
                                    {
                                        bestTarget = kudzu;
                                        bestValue = kudzu.charge;
                                    }
                                }
                                else
                                {
                                    if (kudzu.charge / 1.5 > bestValue)
                                    {
                                        bestTarget = kudzu;
                                        bestValue = kudzu.charge / 1.5;
                                    }
                                }
                            }
                        }
                    }

                    if (bestTarget != null)
                        sh.supportedMilitary.task = new Task_Season_RazeKudzu(bestTarget, sh.supportedMilitary);

                }
            }
        }

    }
}
