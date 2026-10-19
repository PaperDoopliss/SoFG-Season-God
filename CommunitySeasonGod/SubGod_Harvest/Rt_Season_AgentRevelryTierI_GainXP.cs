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
    public class Rt_Season_AgentRevelryTierI_GainXP : Ritual
    {
        public UA caster;
        public static int cost = 1;
        public static int amountTake = 25;
        public static int amountGive = 20;

        public Rt_Season_AgentRevelryTierI_GainXP(Location loc, UA parent)
            : base(loc)
        {
            caster = parent;
        }

        public override string getName()
        {
            return "Fey Experience";
        }

        public override string getCastFlavour()
        {
            return "";
        }

        public override string getDesc()
        {
            return "Spend "+cost+" Mystic Bounty to convert "+amountTake+" of a Fey Presence charge into "+ amountGive + " XP.";
        }

        public override string getRestriction()
        {
            return "Costs "+cost+" Mystic Bounty. Requires a location with a Fey Presence modifier with >= "+amountTake+"% charge.";
        }

        public override double getProfile()
        {
            return 2.0;
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
            return 0;
        }

        public override int getCompletionProfile()
        {
            return 1;
        }

        public override bool validFor(UA ua)
        {
            T_MysticBounty trait = ua.person.traits.OfType<T_MysticBounty>().FirstOrDefault();
            if (trait != null)
            {
                if(trait.amount >= cost)
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
            cast.person.receiveXP(amountGive);

            Pr_FeyPresence presence = cast.location.properties.OfType<Pr_FeyPresence>().FirstOrDefault();
            if (presence != null)
            {
                presence.charge -= amountTake;
            }

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

        public override bool valid()
        {
            Pr_FeyPresence presence = this.location.properties.OfType<Pr_FeyPresence>().FirstOrDefault();
            if (presence != null)
            {
               if(presence.charge >= amountTake)
               {
                    return true;
               }
            }
            return false;
        }

        public override int[] buildPositiveTags()
        {
            return new int[0];
        }

        public override int[] buildNegativeTags()
        {
            return new int[0];
        }
    }
}
