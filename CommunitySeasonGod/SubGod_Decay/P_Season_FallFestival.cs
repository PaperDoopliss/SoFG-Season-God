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
            return $"Cast on an infiltrated settlement to immediately set the location <b>fey presence</b> to 50%.";
        }

        public override string getFlavour()
        {
            return "Though the festival was strange and pagan, no voice rose in dissent as the pumpkins were carted in and the straw pyres erected in every square, and thus no questions were asked. Faces both uncommonly new and old danced into the night, the auburn haired beauty never far from sight, as she leads the partying townsfolk in willingly ushering in a season of beautiful leaves and withered crops.";
        }

        public override string getRestrictionText()
        {
            return $"Must target an infiltrated settlement with less than 50% <b>fey presence</b>.";
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
            Pr_FeyPresence fey = loc.properties.OfType<Pr_FeyPresence>().FirstOrDefault();
            if ((fey == null || fey.charge < 50) && settlement.isInfiltrated) return true;
            return false;
        }

        public override int getCost() => 1;

        public override void cast(Location loc)
        {
            base.cast(loc);
            Pr_FeyPresence fey = loc.properties.OfType<Pr_FeyPresence>().FirstOrDefault();
            if (fey == null)
            {
                fey.charge = 50;
                loc.properties.Add(fey);
            }
            else if (fey.charge < 50) {
                fey.charge = 50;
            }
        }

    }
}
