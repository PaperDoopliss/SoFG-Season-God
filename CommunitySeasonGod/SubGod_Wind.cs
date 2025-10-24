using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class SubGod_Wind : SubGod
    {
        public Map map;
        public SubGod_Wind(God_Season god, Map map)
            : base(god, map)
        {
            Powers.Add(new P_Season_WindCurrent(map));
            PowerLevelReqs.Add(0);
            BonusPowers.Add(new P_Season_HuntBonus(map));
            BonusPowerLevelReqs.Add(0);
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
