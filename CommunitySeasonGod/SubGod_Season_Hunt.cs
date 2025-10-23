using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class SubGod_Season_Hunt : Season_SubGod
    {
        public Map map;
        public SubGod_Season_Hunt(Map map)
        {
            this.map = map;
            powers.Add(new P_Season_HuntCamouflage(map));
            powerLevelReqs.Add(0);
            bonusPowers.Add(new P_Season_HuntBonus(map));
            bonusPowerLevelReqs.Add(0);
        }

        public override string GetName()
        {
            return "Master of the Hunt";
        }

        public override string GetKeywords()
        {
            return "Agents, Heroes";
        }

        public override string GetEventPath()
        {
            return "comseason.shift_hunt";
        }

        public override string GetSpritePath()
        {
            return "comseason.portrait_hunt.png";
        }

    }
}
