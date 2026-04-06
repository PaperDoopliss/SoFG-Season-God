using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class SubGod_Bloom : SubGod
    {
        public Map map;
        public List<Property> crops;

        public SubGod_Bloom(God_Season god, Map map)
            : base(god, map)
        {
            crops = new List<Property>();


            BonusPowers.Add(new P_Season_AbundantCrops(map));
            BonusPowerLevelReqs.Add(0);

            Powers.Add(new P_Season_FeyCrops(map));
            PowerLevelReqs.Add(0);
            Powers.Add(new P_Season_BloomingFields(map));
            PowerLevelReqs.Add(0);
            Powers.Add(new P_Season_Fertilization(map));
            PowerLevelReqs.Add(1);
            Powers.Add(new P_Season_FeyBargain(map));
            PowerLevelReqs.Add(3);
            Powers.Add(new P_Season_DreamingKudzu(map));
            PowerLevelReqs.Add(5);
            Powers.Add(new P_Season_BlunderTheThrone(map));
            PowerLevelReqs.Add(6);
            Powers.Add(new P_Season_HopefulPromises(map));
            PowerLevelReqs.Add(8);
            Powers.Add(new P_Season_VerdantGate(map));
            PowerLevelReqs.Add(9);
        }

        public override string GetName()
        {
            return "Niece of Blooming Fields";
        }

        public override string GetKeywords()
        {
            return "Growth, Madness";
        }

        public override string GetEventPath()
        {
            return "ComSeasonGod.shift_bloom";
        }

        public override string GetSpritePath()
        {
            return "ComSeasonGod.portrait_bloom.png";
        }

        public override Sprite GetSupplicantSprite()
        {
            return EventManager.getImg("ComSeasonGod.unit_supplicant_bloom.png");
        }

        public override bool HasSupplicantStartingTraits()
        {
            return true;
        }

        public override List<Trait> GetSupplicantStartingTraits()
        {
            return new List<Trait>() { new T_Season_VerdantTraveler(), new T_Season_VerdantImmortality(), new T_Season_VerdantHerbalist() };
        }

        public override string GetVictoryMessage(int victoryMode)
        {
            switch (victoryMode)
            {
                case 0:
                    return "The Niece's creations worm their way into every corner of the world, proliferating out of control and blotting out the light. Vines wrap around every building and flowers root in human bodies. A symbiosis emerges where society molds itself around the new life, and resisting the Niece's touch becomes unimaginable.";
                case 1:
                    return "The world shrinks and warps as the Niece's alien innocence imposes itself on humanity. Entire cities sink into dream as the kudzu consumes them, the people kept alive by a strange photosynthesis. For all the world is unrecognizable, life continues, and the world becomes an extension of the Niece's will.";
                case 2:
                    return "The Niece's vision has fully taken root as the nations of the world flock to her banner. Her otherworldly crops sustain the empire, the Dreaming Kudzu suppresses dissent, and her touch smooths over the horrible wounds of conquest. The people are healthy and the world is harmonious as humanity embraces its irrelevance.";
                case 3:
                    return "The Niece's new growth crowds out all competition, including society itself. Edible crops are choked out, vines tear down buildings, and the blood of livestock nourishes the soil. The devastation is wholly without malice, the inevitable consequence of invasive species left unchecked.";
                case 4:
                    return "Winter comes even for creatures of growth. The Niece's creations can adapt to an extent, but the civilizations of the world could not. The world shifts gracefully to a lower-energy state, and humanity withers away in the face of it.";
                case 5:
                    return "New coral and algae proliferate under the sea. The oceans teem with nutritious fish, and humanity follows them to the coast for an easier life. Under the Niece's will, slipping under the waves is the most natural-seeming thing in the world as the Deep Ones fully eclipse land-walking society.";
                default:
                    return "";
            }
        }

        public override void OnActivate(Map map, SubGod previousSubGod, bool enteredNaturally)
        {
            base.OnActivate(map, previousSubGod, enteredNaturally);

            TurnTick_Active(map);
        }

        public override void OnDeactivate(Map map, SubGod nextSubGod, bool exitedNaturally)
        {
            base.OnDeactivate(map, nextSubGod, exitedNaturally);

            for (int i = 0; i < map.overmind.agents.Count; i++)
            {
                if (map.overmind.agents[i] is UAE uae)
                {
                    if (uae.person != null)
                    {
                        foreach (Trait t in uae.person.traits)
                        {
                            if (t is T_Season_VerdantHerbalist)
                            {
                                UA hero = UAG_Season_ReleasedAgent.replaceWithHero(uae);
                                map.addUnifiedMessage(hero, null, "Rebellious Supplicant", "As the Niece of Blooming Fields wanes, " + uae.getName() + " bucks off the next Noble's dominion. They consider their Verdant Herbalism a sacred duty, and will continue treating humanity's ailments as a hero. Given the side-effects of the treatments, it may not be a bad thing.", "REBELLIOUS SUPPLICANT");
                                break;
                            }
                        }
                    }
                }
            }
        }

        public override void TurnTick_Active(Map map)
        {
            base.TurnTick_Active(map);

            for (int i = 0; i < crops.Count; i++)
            {
                if (crops[i].location.properties.Contains(crops[i]) == false)
                {
                    crops.RemoveAt(i);
                    i--;
                }
            }
        }

    }
}
