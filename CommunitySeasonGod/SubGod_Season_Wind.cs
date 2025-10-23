using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class SubGod_Season_Wind : SubGod
    {
        public Map map;
        public SubGod_Season_Wind(Map map)
        {
            this.map = map;
            powers.Add(new P_Season_WindCurrent(map));
            powerLevelReqs.Add(0);
            bonusPowers.Add(new P_Season_HuntBonus(map));
            bonusPowerLevelReqs.Add(0);
        }

        public override string GetName()
        {
            return "Painter of Winds";
        }

        public override string GetKeywords()
        {
            return "Modifiers, Movement";
        }

        public override string GetEventPath()
        {
            return "comseason.shift_wind";
        }

        public override string GetSpritePath()
        {
            return "comseason.portrait_wind.png";
        }

    }
}
