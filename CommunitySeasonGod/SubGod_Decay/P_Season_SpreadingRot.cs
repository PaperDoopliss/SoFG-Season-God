using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_SpreadingRot : P_Season
    {
        private const int THRESHOLD_ONE = 20;
        private const int THRESHOLD_TWO = 30;

        public P_Season_SpreadingRot(Map map) : base(map) { }

        public override string getName() => "Spreading Rot";

        public override string getDesc()
        {
            return $@"Cast on a <b>wilted grove</b> to spawn armies of wilted hordes based on its defence.

• <b>1-{THRESHOLD_ONE}:</b> 1 army.
• <b>{THRESHOLD_ONE + 1}-{THRESHOLD_TWO}:</b> 2 armies.
• <b>{THRESHOLD_ONE + 1}+:</b> 3 army.";
        }

        public override string getFlavour()
        {
            return "Those who've felt our lady of wiltings's caress feel no suffering, require no sustenance. Some may say she has made abominations of them, but she knows the truth. They are art, a work of fleeting beauty that deserves to be grown and tended.";
        }

        public override string getRestrictionText()
        {
            return $"Target location must have a farming community.";
        }

        public override Sprite getIconFore()
        {
            return map.world.iconStore.mushroomFarms;
        }

        public override bool validTarget(Unit unit)
        {
            return false;
        }

        public override bool validTarget(Location loc)
        {
            if (!(loc.settlement is Set_WiltedGrove)) return false;
            return false;
        }

        public override int getCost() => 3;

        public override void cast(Location loc)
        {
            base.cast(loc);
            Set_WiltedGrove settlement = (Set_WiltedGrove)loc.settlement;

            if (settlement == null) return;

            int numArmies = 0;

            if (settlement.Size <= THRESHOLD_ONE)
            {
                numArmies = 1;
            }
            else if (settlement.Size > THRESHOLD_ONE && settlement.Size < THRESHOLD_TWO) {
                numArmies = 1;
            }
            else if (settlement.Size > THRESHOLD_TWO)
            {
                numArmies = 3;
            }

            for (int i = 0; i < numArmies; i++)
            {
                this.map.units.Add(new UM_WiltedHorde(loc, loc.soc, settlement, settlement.Size));
            }
        }

    }
}
