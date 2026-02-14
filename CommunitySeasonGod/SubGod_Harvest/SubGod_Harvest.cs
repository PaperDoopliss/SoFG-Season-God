using System;
using Assets.Code;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class SubGod_Harvest : SubGod
    {
        public Map map;
        public SubGod_Harvest(God_Season god, Map map)
            : base(god, map)
        {
            Powers.Add(new P_Season_WindCurrent(map));
            PowerLevelReqs.Add(0);
            BonusPowers.Add(new P_Season_HuntBonus(map));
            BonusPowerLevelReqs.Add(0);
        }
        public override string GetName()
        {
            return "Uncle of the Harvest";
        }

        public override string GetKeywords()
        {
            return "Co-operation, Fey Presence";
        }

        public override bool HasSupplicantStartingTraits()
        {
            return true;
        }

        public override List<Trait> GetSupplicantStartingTraits()
        {
            List<Trait> traits = new List<Trait>();
            traits.Add(new T_JoyfulPresence());
            traits.Add(new T_VeilPiercer());
            traits.Add(new T_MartyrOfRevelry());
            return traits;
        }
    }


}
