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
            Powers.Add(new P_Season_RestlessAnticipation(map)); //adds lingering resentment to a location with 50% Fey Presence
            PowerLevelReqs.Add(0);
            Powers.Add(new P_Season_RevelerTierI(map)); //adds a jolly reveler who spreads fey presence
            PowerLevelReqs.Add(1);
            Powers.Add(new P_Season_HarvestHospitality(map));
            PowerLevelReqs.Add(3);
            Powers.Add(new P_Season_HarvestMysticBounty(map)); //gives an agent the mystic bounty trait and the first set of rituals it can do
            PowerLevelReqs.Add(3);
            Powers.Add(new P_Season_RevelerTierII(map)); //adds a trickster reveler who spreads fey presence
            PowerLevelReqs.Add(4);
            Powers.Add(new P_Season_HarvestMysticBountyTierII(map)); //TODO: unlocks second tier of mystic bounty rituals
            PowerLevelReqs.Add(5);
            Powers.Add(new P_Season_HarvestVeilbreaker(map));
            PowerLevelReqs.Add(5);
            Powers.Add(new P_Season_RevelerTierIII(map)); //adds a lord reveler who spreads fey presence
            PowerLevelReqs.Add(7);
            Powers.Add(new P_Season_HarvestFestival(map));
            PowerLevelReqs.Add(9);
            BonusPowers.Add(new P_Season_HarvestBonus(map));
            BonusPowerLevelReqs.Add(0);
        }
        public override string GetName()
        {
            return "Uncle of the Harvest";
        }

        public override string GetKeywords()
        {
            return "Unrest, Disruption, Revelry, Fey Presence";
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
