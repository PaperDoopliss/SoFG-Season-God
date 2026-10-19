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
    public class Rt_Season_AgentRevelryTierI_ArcaneSecret : Ritual
    {
        public UA caster;
        public static int cost = 5;

        public Rt_Season_AgentRevelryTierI_ArcaneSecret(Location loc, UA parent)
            : base(loc)
        {
            caster = parent;
        }

        public override string getName()
        {
            return "Peer Past the Veil";
        }

        public override string getCastFlavour()
        {
            return "";
        }

        public override string getDesc()
        {
            return "Spends "+cost+" Mystic Bounty to gain 1 Arcane Knowledge, which can be used to upgrade your agent's magical abilities.";
        }

        public override string getRestriction()
        {
            return "Costs "+cost+" Mystic Bounty";
        }

        public override double getProfile()
        {
            return 5.0;
        }

        public override double getMenace()
        {
            return 0.0;
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.LORE;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            msgs?.Add(new ReasonMsg("Stat: Lore", Math.Max(1, unit.getStatLore())));
            return Math.Max(1, unit.getStatLore());
        }

        public override double getComplexity()
        {
            return 30.0;
        }

        public override int getCompletionMenace()
        {
            return 0;
        }

        public override int getCompletionProfile()
        {
            return 2;
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
            T_ArcaneKnowledge arcane = cast.person.traits.OfType<T_ArcaneKnowledge>().FirstOrDefault();
            if (arcane != null)
            {
                arcane.level++;
            }
            else
            {
                arcane = new T_ArcaneKnowledge();
                arcane.level = 1;
                cast.person.receiveTrait(arcane);
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
