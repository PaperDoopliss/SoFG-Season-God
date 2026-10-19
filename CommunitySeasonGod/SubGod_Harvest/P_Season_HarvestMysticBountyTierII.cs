using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_HarvestMysticBountyTierII : P_Season
    {

        //public static double amountTake = 50;
        //public static int amountGive = 4;
        public P_Season_HarvestMysticBountyTierII(Map map)
            : base(map)
        {

        }

        public override string getName()
        {
            return "[PASSIVE] Harvest Preparation";
        }

        public override bool isPassiveOnly()
        {
            return true;
        }

        public override string getDesc()
        {
            return "[DEBUG: Currently doesn't do anything] Unlocks additional rituals for agents with the Mystic Harvest trait.";
        }

        public override string getFlavour()
        {
            return "Eventually the festival must slow down, ever so slightly, as once again people look to the stockpiles for future months and the almost certain need to prepare for the ending of this season of plenty.";
        }

        public override string getRestrictionText()
        {
            return "";
        }

        public override Sprite getIconFore()
        {
            return map.world.iconStore.give;
        }
    }
}
