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
            Powers.Add(new P_Season_GaleForce(map));
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

        public override string GetVictoryMessage(int victoryMode)
        {
            switch (victoryMode)
            {
                case 0:
                    return "The souls of humanity are blotted out by the Painter's grand work. The world as it was just a few years ago is a pale, hollow thing, the routines of humanity feeling meaningless before the dark whorls and flows that define the new world.";
                case 1:
                    return "The smudging of reality along the Painter's brushstrokes can't be borne by the human mind. The assumptions and simplicity that allowed society to form are peeled away. The resulting collapse is absolute as the predictable and sane age of humanity comes to an end.";
                case 2:
                    return "The lines on the map shift to reflect the Painter's work as her empire spans the world. The empire is the eye of an all-consuming storm, the surrounding world wracked by devastation as refugees flock to its relative safety. The brushstrokes settle with the weight of absolute authority as the Painter's armies crush the free nations of the world.";
                case 3:
                    return "The Painter's work has fully crowded out the life that used to dominate the world. The remnants of society are torn apart by hurricane-force winds as plagues propagate out of control in those corners the wind can't reach. The world is a blank canvas for the eldritch god's will.";
                case 4:
                    return "The world finally goes still as the last fires are torn away in the frigid wind. Now unopposed by squirming mortals, the eldritch god's cold mind can fully unfold across the world they used to call home.";
                case 5:
                    return "The Painter carries her abyssal children across the world on underwater currents. They adapt better to the fey's will than their suface-dwelling cousins, building out new societies at depths unreachable to the heroes who might wish to stop them.";
                default:
                    return "";
            }

        }

    }
}
