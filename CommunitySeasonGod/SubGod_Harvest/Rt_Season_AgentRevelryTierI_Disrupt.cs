using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.Code.Challenge;

namespace CommunitySeasonGod
{
    public class Rt_Season_AgentRevelryTierI_Disrupt : Ritual
    {
        public UA caster;
        public static int cost = 5;
        public Rt_Season_AgentRevelryTierI_Disrupt(Location loc, UA parent)
            : base(loc)
        {
            caster = parent;
        }

        public override string getName()
        {
            return "Spontaneous Revelry";
        }

        public override string getCastFlavour()
        {
            return "";
        }

        public override string getDesc()
        {
            return "Disrupts nearby heroes for 10 turns. The number of heroes that can be affected is based on the agent's Lore.";
        }

        public override string getRestriction()
        {
            return "Costs 5 Mystic Bounty";
        }

        public override double getProfile()
        {
            return 10.0;
        }

        public override double getMenace()
        {
            return 0.0;
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.OTHER;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            //msgs?.Add(new ReasonMsg("Stat: Lore", Math.Max(1, unit.getStatLore())));
            //return Math.Max(1, unit.getStatLore());
            return 1;
        }

        public override double getComplexity()
        {
            return 3.0;
        }

        public override int getCompletionMenace()
        {
            return 1;
        }

        public override int getCompletionProfile()
        {
            return 3;
        }

        public override void onBegin(Unit unit)
        {
            base.onBegin(unit);
        }

        public override bool validFor(UA ua)
        {
            T_MysticBounty trait = ua.person.traits.OfType<T_MysticBounty>().FirstOrDefault();
            if (trait != null)
            {
                if (trait.amount >= cost)
                {
                    return true;
                }
            }

            return false;
        }

        public override Sprite getSprite()
        {
            return map.world.iconStore.enshadow;
        }

        public override int isGoodTernary()
        {
            return 0;
        }

        public override bool isIndefinite()
        {
            return false;
        }

        public override void complete(UA cast)
        {
            bool foundCandidate = false;
            List<UA> list = new List<UA>();

            foreach (Unit u in cast.location.units)
            {
                if (u != null)
                {
                    if (u.isCommandable())
                    {
                        continue;
                    }

                    if (u == cast)
                    {
                        continue;
                    }

                    if (u is UA ua && u is UAG)
                    {
                        list.Add(ua);
                    }
                }
            }

            foreach (Location neighbor in location.getNeighbours())
            {
                if (neighbor != null)
                {
                    foreach (Unit u in neighbor.units)
                    {
                        if (u != null)
                        {
                            if (u.isCommandable())
                            {
                                continue;
                            }

                            if (u == cast)
                            {
                                continue;
                            }

                            if (u is UA ua && u is UAG)
                            {
                                list.Add(ua);
                            }
                        }
                    }
                }
            }

            if (list.Count > 0)
            {
                int maxNum = cast.getStatLore();

                if (maxNum > list.Count)
                {
                    maxNum = list.Count;
                }

                for (int i = 0; i < maxNum; i++)
                {
                    int num = Eleven.random.Next(list.Count - 1);
                    UA candidate = null;
                    if (list[num] != null)
                    {
                        candidate = list[num];
                    }
                    else
                    {
                        candidate = list[0];
                    }

                    if (candidate != null)
                    {
                        candidate.task = null;
                        candidate.task = new Task_Disrupted(10);
                        list.Remove(candidate);
                        map.addUnifiedMessage(candidate, cast, "Hero Disrupted by Agent Revelry", candidate.getName() + " has been disrupted by the spontaneous revelry of " + cast.getName() + "!", "SPONTANEOUS REVELRY");
                        foundCandidate = true;
                    }
                    else
                    {
                        break;
                    }
                }

            }

            if (foundCandidate)
            {
                T_MysticBounty trait = cast.person.traits.OfType<T_MysticBounty>().FirstOrDefault();
                if (trait != null)
                {
                    trait.amount -= cost;
                    if (trait.amount < 0)
                    {
                        trait.amount = 0;
                    }
                }
            }
        }

        public override bool valid()
        {
            return true;
        }

        public override int[] buildPositiveTags()
        {
            return new int[1] { Tags.DISCORD };
        }

        public override int[] buildNegativeTags()
        {
            return new int[0];
        }
    }
}
