using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class SubGod_Decay : SubGod
    {
        public SubGod_Decay(God_Season god, Map map)
            : base (god, map)
        {
            // Powers.Add(new P_Season_HuntCamouflage(map));
            PowerLevelReqs.Add(0);
            // BonusPowers.Add(new P_Season_HuntBonus(map));
            BonusPowerLevelReqs.Add(0);
        }

        public override string GetName()
        {
            return "The Lady of Wilting Leaves";
        }

        public override string GetKeywords()
        {
            return "Famine, Recycling, Armies";
        }

        //public override string GetEventPath()
        //{
        //    return "ComSeasonGod.shift_hunt";
        //}

        public override string GetSpritePath()
        {
            return "ComSeasonGod.portrait_hunt.png";
        }

    }
}
