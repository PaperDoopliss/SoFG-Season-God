/*using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Ch_SolEntity_AddTinder : Challenge
    {
        public UA caster;

        public Ch_SolEntity_AddTinder(Location loc, UA parent)
            : base(loc)
        {
            caster = parent;
        }
        public override string getName()
        {
            return "Add Tinder";
        }

        public  string getFlavor()
        {
            return "Solar Entities tends to aid their underground brethren at times.";
        }
        public override double getProfile()
        {
            return 5.0;
        }

        public override double getMenace()
        {
            return 8.0;
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
            return 40.0;
        }

        public override int getCompletionMenace()
        {
            return 8;
        }

        public override int getCompletionProfile()
        {
            return 5;
        }

        public void complete(Location loc)
        {
            foreach (Property prop in loc.properties)
            {
                if (prop is Pr_Cthonians)
                {
                    prop.charge += 20;
                }
            }
        }

        public override Sprite getSprite()
        {
            return map.world.iconStore.crusade;
        }

        public override int isGoodTernary()
        {
            return -1;
        }
    }
}
*/