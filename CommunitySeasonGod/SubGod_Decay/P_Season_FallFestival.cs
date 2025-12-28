using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_FallFestival : P_Season
    {
        public P_Season_FallFestival(Map map) : base(map) { }

        public override string getName() => "Fall Festival";

        public override string getDesc()
        {
            return $"Cast on an infiltrated settlement to immedaitely set the location <b>{DecayConsts.Wilting.ToLower()}</b> to 50%.";
        }

        public override string getFlavour()
        {
            return "Though the festival was strange and pagan, no voice rose in dissent as the pumpkins were carted in and the straw pyres erected in every square, and thus no questions were asked. Faces both uncommonly new and old danced into the night, the auburn haired beauty never far from sight, as she leads the partying townsfolk in willingly ushering in a season of beautiful leaves and withered crops.\r\n";
        }

        public override string getRestrictionText()
        {
            return $"Must target be a settlement with a greater <b>{DecayConsts.Wilting.ToLower()}</b> modifier than its current population.";
        }

        public override Sprite getIconFore()
        {
            return map.world.iconStore.foreverDying;
        }

        public override bool validTarget(Unit unit)
        {
            return false;
        }

        public override bool validTarget(Location loc)
        {
            if (!(loc.settlement is SettlementHuman)) return false;
            var settlement = (SettlementHuman)loc.settlement;
            var fey = loc.properties.FirstOrDefault(p => p.getInvariantName() == "Fey Presence");
            if ((fey == null || fey.charge < 50) && settlement.isInfiltrated) return true;
            return false;
        }

        public override int getCost() => 1;

        public override void cast(Location loc)
        {
            base.cast(loc);
            loc.properties.Add();
        }

    }
}
