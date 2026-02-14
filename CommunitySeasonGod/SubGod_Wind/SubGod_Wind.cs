using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class SubGod_Wind : SubGod
    {
        public Map map;
        public SubGod_Wind(God_Season god, Map map)
            : base(god, map)
        {

            BonusPowers.Add(new P_Season_ArtisticFlourish(map));
            BonusPowerLevelReqs.Add(0);

            Powers.Add(new P_Season_WindCurrent(map));
            PowerLevelReqs.Add(0);
            Powers.Add(new P_Season_TumultuousGale(map));
            PowerLevelReqs.Add(1);
            Powers.Add(new P_Season_Typhoon(map));
            PowerLevelReqs.Add(3);
            Powers.Add(new P_Season_SmotheringGusts(map));
            PowerLevelReqs.Add(4);
            Powers.Add(new P_Season_VoicesOnTheWind(map));
            PowerLevelReqs.Add(5);
            Powers.Add(new P_Season_EmpoweredBrushstrokes(map));
            PowerLevelReqs.Add(6);
            Powers.Add(new P_Season_PlagueWind(map));
            PowerLevelReqs.Add(7);
            Powers.Add(new P_Season_HurricaneOfCreation(map));
            PowerLevelReqs.Add(8);
            Powers.Add(new P_Season_PrimordialTempest(map));
            PowerLevelReqs.Add(9);
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
            return "ComSeasonGod.shift_wind";
        }

        public override string GetSpritePath()
        {
            return "ComSeasonGod.portrait_wind.png";
        }

        public override Sprite GetSupplicantSprite()
        {
            return EventManager.getImg("ComSeasonGod.unit_supplicant_wind.png");
        }

        public override bool HasSupplicantStartingTraits()
        {
            return true;
        }

        public override List<Trait> GetSupplicantStartingTraits()
        {
            return new List<Trait>() { new T_Season_BolsteredCurrents(), new T_Season_ExitStrategy(), new T_Season_WhispersOnTheWind() };
        }

    }
}
