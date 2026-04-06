using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Act_Season_KudzuQuarantine : Assets.Code.Action
    {

        public Act_Season_KudzuQuarantine(Location loc) : base(loc)
        {
        }

        public override string getName()
        {
            return "Contain Kudzu";
        }

        public override string getShortDesc()
        {
            return "Aware rulers and those who dislike Madness can impose quarantine to increase the amount of Dreaming Kudzu required before it can spread to other locations. Requires " + map.param.act_quarantineGoldCost + " <b>gold</b>.";
        }

        public override Sprite getIconFore()
        {
            return map.world.iconStore.quarantine;
        }

        public override Sprite getIconBack()
        {
            return null;
        }

        public override double getUtility(SettlementHuman hum, Person ruler, List<ReasonMsg> reasons)
        {
            double utility = base.getUtility(hum,ruler,reasons);
            utility -= 20;
            reasons?.Add(new ReasonMsg("Base reluctance", -20));

            if (hum.location.soc is Society society)
            {

                double num2 = 0;
                double num3 = 0;
                double num4 = 0;

                foreach (Location lastTurnLoc in society.lastTurnLocs)
                {

                    double kudzuInLocation = 0;
                    foreach (Property pr in lastTurnLoc.properties)
                    {
                        if (pr is Pr_Season_DreamingKudzu)
                            kudzuInLocation += pr.charge;
                    }
                    

                    if (kudzuInLocation > 0.0)
                    {
                        foreach (Location neighbour in lastTurnLoc.getNeighbours())
                        {

                            double kudzuInNeighbour = 0;
                            foreach (Property pr in neighbour.properties)
                            {
                                if (pr is Pr_Season_DreamingKudzu)
                                    kudzuInNeighbour += pr.charge;
                            }

                            if (neighbour.soc == society && neighbour.settlement is SettlementHuman && kudzuInNeighbour == 0.0)
                            {
                                num4 += 1.0;
                            }
                        }
                    }

                    foreach (Property property in lastTurnLoc.properties)
                    {
                        if (property is Pr_Quarantine)
                        {
                            num3 += property.charge;
                        }
                    }
                }

                if (society.lastTurnLocs.Count > 0)
                {
                    num3 /= (double)society.lastTurnLocs.Count;
                    num4 /= (double)society.lastTurnLocs.Count;
                }

                num2 = num4 * (double)map.param.utility_soc_applyQuarantine;
                if (num2 > 0.0)
                {
                    utility += num2;
                    reasons?.Add(new ReasonMsg("Locations at risk of spread", num2));
                }

                num2 = num3 * (double)map.param.utility_soc_alreadyHaveQuarantine;
                if (num2 < 0.0)
                {
                    utility += num2;
                    reasons?.Add(new ReasonMsg("Already Quarantined", num2));
                }

                double num5 = 0.0;
                foreach (SocialGroup socialGroup in map.socialGroups)
                {
                    if (!socialGroup.isGone() && map.getStepDist(society, socialGroup) < 4)
                    {
                        double threatIgnoringDistance = society.getSovreign().getThreatIgnoringDistance(socialGroup);
                        if (threatIgnoringDistance > num5)
                        {
                            num5 = threatIgnoringDistance;
                        }
                    }
                }

                num2 = num5 * (double)map.param.utility_soc_avoidQuarantineForMilitaryReasons * -1.0;
                if (num2 < 0.0)
                {
                    utility += num2;
                    reasons?.Add(new ReasonMsg("Need prosperity for military", num2));
                }
            }

            return utility;
        }

        public override bool valid(Person ruler, SettlementHuman settlementHuman)
        {

            if (ruler.gold < map.param.act_quarantineGoldCost)
                return false;

            if (ruler.awareness >= 1)
                return true;

            if (ruler.getTagRanking(Tags.MADNESS) <= -1)
                return true;

            return false;
        }

        public override int[] getNegativeTags()
        {
            return new int[2] { Tags.MADNESS, map.soc_dark.index + 20000 };
        }


        public override void complete()
        {

            if (location.settlement is SettlementHuman sh && sh.ruler != null)
            {

                sh.ruler.addGold(-map.param.act_quarantineGoldCost);
                foreach (Location location in map.locations)
                {
                    if (location.soc != sh.ruler.society || !(location.settlement is SettlementHuman))
                    {
                        continue;
                    }

                    Pr_Quarantine pr_Quarantine = null;
                    bool plagueAlreadyPresent = false;
                    foreach (Property property in location.properties)
                    {
                        if (property is Pr_Quarantine pr_Quarantine2)
                        {
                            pr_Quarantine = pr_Quarantine2;
                            break;
                        }
                        else if (property is Pr_Plague)
                            plagueAlreadyPresent = true;
                    }

                    if (!plagueAlreadyPresent)
                        continue;

                    if (pr_Quarantine == null)
                    {
                        pr_Quarantine = new Pr_Quarantine(location);
                        location.properties.Add(pr_Quarantine);
                    }

                    pr_Quarantine.charge = 25.0;
                }
            }
        }



    }
}
