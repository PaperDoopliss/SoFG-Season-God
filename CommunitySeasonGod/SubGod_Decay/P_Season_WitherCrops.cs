using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_WitherCrops : P_Season
    {
        private const int DEVO_AMOUNT = 50;

        public P_Season_WitherCrops(Map map) : base(map) { }

        public override string getName() => "Wither Crops";

        public override string getDesc()
        {
            return $"Cast on a location with a farming community to add {DEVO_AMOUNT} <b>enviromental devastation</b>.";
        }

        public override string getFlavour()
        {
            return "She comes in the night, at the height of the harvest moon - entrancing all that see her with her striking auburn locks as she walks gayly through the wheat fields. Their eyes following her movements so closely, they scarcely notice their crops withering and dying beneath her heels.";
        }

        public override string getRestrictionText()
        {
            return $"Target location must have a farming community.";
        }

        public override Sprite getIconFore()
        {
            return map.world.iconStore.farms;
        }

        public override bool validTarget(Unit unit)
        {
            return false;
        }

        public override bool validTarget(Location loc)
        {
            if (!(loc.settlement is SettlementHuman)) return false;
            var settlement = (SettlementHuman)loc.settlement;
            if (settlement.subs.OfType<Sub_Farms>().FirstOrDefault() != null) return true;
            return false;
        }

        public override int getCost() => 2;

        public override void cast(Location loc)
        {
            base.cast(loc);
            Pr_Devastation devastation = loc.properties.OfType<Pr_Devastation>().FirstOrDefault();

            if (devastation == null)
            {
                devastation.charge = DEVO_AMOUNT;
                loc.properties.Add(devastation);
            }
            else if (devastation.charge < 50) {
                devastation.charge += DEVO_AMOUNT;
            }
        }

    }
}
