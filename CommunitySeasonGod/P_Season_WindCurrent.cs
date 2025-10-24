using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_WindCurrent : P_Season
    {
        public P_Season_WindCurrent(Map map) : base(map) { }

        public override string getName()
        {
            return "Wind Current";
        }

        public override string getDesc()
        {
            return "Places a Wind Current in a location next to an ocean location, or that has Fey Presence.";
        }

        public override string getFlavour()
        {
            return "THIS IS SUPER PLACEHOLDER, I JUST NEED POWERS FOR TESTING PURPOSES";
        }

        public override string getRestrictionText()
        {
            return "Must target a location next to an ocean location, or that has Fey Presence.";
        }

        public override Sprite getIconFore()
        {
            return map.world.iconStore.bringSnow;
        }

        public override bool validTarget(Unit unit)
        {
            return false;
        }

        public override bool validTarget(Location loc)
        {
            return true;
        }

        public override int getCost()
        {
            return 1;
        }

        public override void cast(Location location)
        {
            base.cast(location);

        }


    }
}
