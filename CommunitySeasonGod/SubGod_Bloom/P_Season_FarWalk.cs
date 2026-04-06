using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_FarWalk : P_Season_LimitedCharges
    {
        public Unit unit;
        public P_Season_FarWalk(Map map, Unit unit) : base(map) 
        {
            _maxCharges = 1;
            Charges = 1;
            this.unit = unit;
        }

        public override string getName()
        {
            return "Far Walk";
        }

        public override string getDesc()
        {
            return "Instantly transports " + unit.getName() + " to a location with Fey Crops";
        }

        public override string getFlavour()
        {
            return "They step between two trees and do not step out the other side.";
        }

        public override string getRestrictionText()
        {
            return "Must target a location with Fey Crops.";
        }

        public override Sprite getIconFore()
        {
            return map.world.iconStore.ophanimSwiftOfFoot;
        }

        public override bool validTarget(Location loc)
        {
            if (unit.location == loc)
                return false;

            if (loc.properties.Any(p => p is Pr_Season_FeyCrops))
                return true;

            return false;
        }

        public override int getCost()
        {
            return 0;
        }

        public override void cast(Location location)
        {
            base.cast(location);

            unit.location.units.Remove(unit);
            unit.location = location;
            location.units.Add(unit);

            SpendCharge();
        }

    }
}
