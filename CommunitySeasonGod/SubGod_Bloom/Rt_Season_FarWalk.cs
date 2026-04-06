using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Rt_Season_FarWalk : Ritual
    {
        public Rt_Season_FarWalk(Location loc) : base(loc) { }

        public override double getMenace()
        {
            return 0.0;
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.OTHER;
        }

        public override string getName()
        {
            return "Far Walk";
        }

        public override string getDesc()
        {
            return "Grants a Far Walk power until end of turn. If this power is used on a location with Fey Crops, this unit will move instantly to that location.";
        }

        public override string getCastFlavour()
        {
            return "";
        }

        public override string getRestriction()
        {
            return "Can only be performed on locations with Fey Crops";
        }

        public override double getProfile()
        {
            return 0;
        }
        public override int getSimplificationLevel()
        {
            return 0;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            return 1;
        }

        public override double getComplexity()
        {
            return 1;
        }

        public override bool isIndefinite()
        {
            return true;
        }

        public override Sprite getSprite()
        {
            return map.world.iconStore.ophanimSwiftOfFoot;
        }

        public override int isGoodTernary()
        {
            return 0;
        }

        public override bool validFor(UA ua)
        {
            if (map.overmind.god.powers.Any(p => p is P_Season_FarWalk))
                return false;

            foreach (Property pr in ua.location.properties)
            {
                if (pr is Pr_Season_FeyCrops)
                    return true;
            }

            return false;
        }

        public override void onImmediateBegin(UA uA)
        {
            base.onImmediateBegin(uA);
            map.world.audioStore.playActivate();
            map.overmind.god.powers.Add(new P_Season_FarWalk(map, uA));
            map.overmind.god.powerLevelReqs.Add(0);
            uA.task = null;
        }

        public override void complete(UA u)
        {
        }

        public override bool valid()
        {
            return true;
        }


    }
}
