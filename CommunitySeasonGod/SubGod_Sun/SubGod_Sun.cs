using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class SubGod_Sun : SubGod
    {

        public static double devastationOnSeasonChange = 10;
        public static float tempIncrease = 0.05f;

        public SubGod_Sun(God_Season god, Map map)
             : base (god, map)
        {
            Powers.Add(new P_Season_Erosion(map));
            PowerLevelReqs.Add(0);

            Powers.Add(new P_Season_SunsEmbrace(map));
            PowerLevelReqs.Add(/*1*/0);

            Powers.Add(new P_Season_Wildfire(map));
            PowerLevelReqs.Add(/*2*/0);

            Powers.Add(new P_Season_FertilizingAsh(map));
            PowerLevelReqs.Add(/*3*/0);

            Powers.Add(new P_Season_AshCloud(map));
            PowerLevelReqs.Add(/*4*/0);

            Powers.Add(new P_Season_SunEntity(map));
            PowerLevelReqs.Add(/*5*/0);

            Powers.Add(new P_Season_SolarAbsorption(map));
            PowerLevelReqs.Add(/*6*/0);

            Powers.Add(new P_Season_IdolOfSolus(map));
            PowerLevelReqs.Add(/*7*/0);

            Powers.Add(new P_Season_AshenCircles(map));
            PowerLevelReqs.Add(/*8*/0);

            Powers.Add(new P_Season_HeatWave(map));
            PowerLevelReqs.Add(/*9*/0);

            BonusPowers.Add(new P_Season_FrenziedPrayers(map));
            BonusPowerLevelReqs.Add(0);
        }

        public override bool HasSupplicantStartingTraits()
        {
            return true;
        }

        public override List<Trait> GetSupplicantStartingTraits()
        {
            return new List<Trait>() { new T_Supplicant_PatriarchsWarmth(), new T_Supplicant_SunFathersWrath(), new T_Supplicant_SunGodsRadiance() };
        }

        public override string GetName()
        {
            return "Patriarch of the Sun";
        }

        public override string GetKeywords()
        {
            return "Devastation, Temperature";
        }

        public override string GetEventPath()
        {
            return "ComSeasonGod.shift_sun";
        }

        public override string GetSpritePath()
        {
            return "ComSeasonGod.portrait_sun.png";
        }

        public override Sprite GetSupplicantSprite()
        {
            return EventManager.getImg("ComSeasonGod.unit_supplicant_sun.png");
        }

        public override string GetVictoryMessage(int victoryMode)
        {
            switch (victoryMode)
            {
                case 0:
                    return "The sun burns hot and furious, but its light does not reach the earth. Ash covers the sky, smothering the light and sealing the Patriarch's heat into the land. The sun was no longer a thing of life, instead sealing the world's fate as it enters its last age.";
                case 1:
                    return "The Patriarch could not help but inflict his own madness on the world he reached out to. Human souls shimmer like heat mirages, society losing its shared basis as the world collapses into chaos and war.";
                case 2:
                    return "The banner of the murderous sun could be seen across the world, the land outside the Dark Empire burned into a blasted wasteland. The Patriarch's empire worship him, praying that he spares them from his cruel light and burns away their enemies. Dissenting souls are burned on the pyre to light the empire's darkened streets.";
                case 3:
                    return "The new world is bright and harsh - too harsh for the societies that used to dot the land. Fresh water became a scarcity and then a memory as the heat spread, to the point where even sweating would heat the body up instead of cooling it down. It is beautiful, in its way, but terribly empty.";
                case 4:
                    return "The Patriarch's supposed cultists have completed their strange betrayal. Perhaps they sealed their Patriarch in the firmament against his will so they could channel his powers and rule over the cold, or perhaps what they saw disgusted them so much that defiance was the only thing left to them. ";
                case 5:
                    return "The Patriarch's glare shimmers on the ocean surface, piercing deeper into the water than ever before. The sea floor is dotted with basalt edifices and shrines to the alien sun, nurturing deep society with blooming coral and kelp as the surface burns.";
                default:
                    return "";
            }

        }

        public override void Awaken()
        {
            base.Awaken();

            Location elderTombLoc = null;

            foreach (Location l in World.staticMap.locations)
            {
                if (l.settlement is Set_TombOfGods)
                    elderTombLoc = l;

                if (l.settlement is SettlementHuman)
                {
                    Property.addToPropertySingleShot("The Dry Season", Property.standardProperties.DEVASTATION, 10, l);
                }
                /*foreach (Hex hex in l.territory)
                {
                    hex.transientTempDelta += tempIncrease;
                }*/
            }

            World.staticMap.globalTemporaryTempDelta += tempIncrease;

            if (elderTombLoc != null)
                World.staticMap.addUnifiedMessage(elderTombLoc, null, "The Dry Season", "As the Solar Patriarch awakens, the sun beats down on the world, and moisture sizzles away. The map's temperature has temporarily increased by " + tempIncrease * 100 + "%, and every populated settlement suffers " + devastationOnSeasonChange + " Devastation.", "THE DRY SEASON");

        }

    }
}