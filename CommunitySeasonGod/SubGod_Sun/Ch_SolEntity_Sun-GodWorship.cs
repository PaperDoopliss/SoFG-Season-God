/*using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace CommunitySeasonGod
{
    class Ch_SolEntity_SunGodWorship : Challenge
    {
        public UA caster;

        public Ch_SolEntity_SunGodWorship(Location loc, UA parent)
            : base(loc)
        {
            caster = parent;
        }

        public override string getName()
        {
            return "Sun-God Worship";
        }

        public  string getFlavour()
        {
            return "Solus grins down upon his progeny.";
        }

        public override double getProfile()
        {
            return 5.0;
        }

        public override double getMenace()
        {
            return 3.0;
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
            return 20.0;
        }

        public override int getCompletionMenace()
        {
            return 3;
        }

        public override int getCompletionProfile()
        {
            return 5;
        }

        public void complete()
        {
            //Increase temp by 5 degrees.
            foreach (Property prop in loc.properties)
            {
                //Increase Fey Presence by 10 in Elven Settlements
            }
        }

        public override Sprite getSprite()
        {
            return map.world.iconStore.theProphecy;
        }

        public override int isGoodTernary()
        {
            return -1;
        }
    }
}*/
