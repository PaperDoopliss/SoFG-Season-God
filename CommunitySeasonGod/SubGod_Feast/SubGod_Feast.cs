using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class SubGod_Feast : SubGod
    {
        public Map map;
        public bool upgradeAgent = false;

        public SubGod_Feast(God_Season god, Map map)
            : base(god, map)
        {
            Powers.Add(new P_Season_PaleKnightsSummon(map));
            PowerLevelReqs.Add(0);
            Powers.Add(new P_Season_PaleKnightsRide(map));
            PowerLevelReqs.Add(1);
            Powers.Add(new P_Season_PrepareSpoils(map));
            PowerLevelReqs.Add(1);
            Powers.Add(new P_Season_EnnobleSavage(map));
            PowerLevelReqs.Add(4);
            Powers.Add(new P_Season_SummonArtifact(map));
            PowerLevelReqs.Add(6);
            Powers.Add(new P_Season_EmboldenBlood(map));
            PowerLevelReqs.Add(7);
            Powers.Add(new P_Season_PaleKnightsChevauchee(map));
            PowerLevelReqs.Add(9);
        }

        public override string GetName()
        {
            return "Lord of the Feast";
        }

        public override string GetKeywords()
        {
            return "War, Stat Boosts";
        }

        public override string GetEventPath()
        {
            return "ComSeasonGod.shift_feast";
        }

        public override string GetSpritePath()
        {
            return "ComSeasonGod.portrait_feast.png";
        }

        public override Sprite GetSupplicantSprite()
        {
            return EventManager.getImg("ComSeasonGod.unit_supplicant_feast.png");
        }

        public override bool HasSupplicantStartingTraits()
        {
            return true;
        }

        public override List<Trait> GetSupplicantStartingTraits()
        {
            List<Trait> traits = base.GetSupplicantStartingTraits();

            traits.Add(new T_Season_KnightlyChallenger());
            traits.Add(new T_Season_HonoredHost());
            traits.Add(new T_Season_FeyishReliquary());

            return traits;
        }

        public override void TurnTick_Active(Map map)
        {
            base.TurnTick_Active(map);

            if (upgradeAgent)
            {
                upgradeAgent = false;
                if (map.overmind.agents.Count > 0)
                {
                    int randomAgent = Eleven.random.Next(map.overmind.agents.Count);
                    if (map.overmind.agents[randomAgent].person != null)
                    {
                        if (Eleven.random.NextDouble() <= 0.5)
                        {
                            map.overmind.agents[randomAgent].person.stat_might++;
                            map.addUnifiedMessage(map.overmind.agents[randomAgent].person, null, "Season of the Feast", "As the Lord of the Feast asserts himself on the world, his morbid affections falls on the tiny creatures bound to carry out " + map.overmind.god.getName() + "'s will, infusing them with fey energies.\n\n" + map.overmind.agents[randomAgent].getName() + " gains +1 <b>might</b>.", "SEASON OF THE FEAST");
                        }
                        else
                        {
                            map.overmind.agents[randomAgent].person.stat_command++;
                            map.addUnifiedMessage(map.overmind.agents[randomAgent].person, null, "Season of the Feast", "As the Lord of the Feast asserts himself on the world, his morbid affections falls on the tiny creatures bound to carry out " + map.overmind.god.getName() + "'s will, infusing them with fey energies.\n\n" + map.overmind.agents[randomAgent].getName() + " gains +1 <b>command</b>.", "SEASON OF THE FEAST");

                        }


                    }
                }
            }
        }

        public override void OnDeactivate(Map map, SubGod nextSubGod, bool exitedNaturally)
        {
            base.OnDeactivate(map, nextSubGod, exitedNaturally);

            List<Unit> toRemove = new List<Unit>();
            foreach (Unit unit in map.units)
            {
                if (unit is UAE_Season_PaleKnight || unit is UM_Season_FeyKnights)
                {
                    toRemove.Add(unit);
                }
            }

            foreach (Unit unit in toRemove)
            {
                map.units.Remove(unit);
                unit.location.units.Remove(unit);
            }
        }

        public override void OnActivate(Map map, SubGod previousSubGod, bool enteredNaturally)
        {
            base.OnActivate(map, previousSubGod, enteredNaturally);

            if (enteredNaturally)
            {
                upgradeAgent = true;
            }
        }

        public override string GetVictoryMessage(int victoryMode)
        {
            switch (victoryMode)
            {
                case 0:
                    return "Mankind has fallen to the machinations of a nobility far grander and more decrepit than their own. The great Feyblooded lords rule from massive spires constructed over centuries, demanding neverending tribute from those beneath them. Somehow, it is familiar.";
                case 1:
                    return "The enlightenment that the Fey brought unleashed war and strength that mankind could not comprehend. In the madness of ever growing courts and corruption, the feyblooded lords lost their mind and battled each other for nothing. And with them, so did the world fall.";
                case 2:
                    return "The Lord of the Feast’s task is achieved in full. He stands as Lord over the entire world, human and beast alike. Other fragments of the eldritch god are fully subordinated, and all men praise him as Lord. He walks among mankind adored, battling beasts and innocents alike to slake his desires.";
                case 3:
                    return "There is nothing left. The Feyblooded lords battled themselves until their lineages became extinct, and by then, there was nothing to preserve mankind from the wrath of the fey. While the Lord of the Feast did bend the world to the eldritch god's will, the Pale Knight has been seen wailing as he rides across the ruin of the world.";
                case 4:
                    return "The Painter of Winds has stolen victory from him. Ice was the final plan of the Feast Lord to break humanity, but the chill and the stillness was not compatible with his nature. The Fool Lord is sealed off from his victory, his glory blowing away on the frigid wind.";
                case 5:
                    return "The Deep ones revere the fey far more than their human ancestors. The eldritch god's hold on the earth has never been stronger, as sea and land mix into a lively sludge. Yet, a knight in pale armor still marches across the land, felling any Deep One who attempts to return to the land, eluding anyone summoned to stop him.";
                default:
                    return "";
            }
            
        }

        /*public override List<Trait> GetSupplicantStartingTraits()
        {
            return new List<Trait>() { new T_Season_BolsteredCurrents(), new T_Season_ExitStrategy(), new T_Season_WhispersOnTheWind() };
        }*/

    }
}
