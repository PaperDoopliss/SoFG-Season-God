using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class God_Season : God
    {
        [SerializeField]
        private SubGod _activeSubGod = null;
        public SubGod ActiveSubGod => _activeSubGod;

        [SerializeField]
        private List<SubGod> _subGods = new List<SubGod>();
        public List<SubGod> SubGods => _subGods;

        [SerializeField]
        private List<SubGod> _subGodDeck = new List<SubGod>();
        public List<SubGod> SubGodDeck => _subGodDeck;

        [SerializeField]
        private List<Power> _genericPowers = new List<Power>();
        [SerializeField]
        private List<int> _genericPowerLevelReqs = new List<int>();

        public int turnsRemainingInSeason = Kernel_Season.Instance.opt_seasonLength;
        public bool hasClungToThrone = false;
        public bool lastTransitionWasTimed = false;

        public override void setup(Map map)
        {
            base.setup(map);

            // Instantiate and store each SubGod that is enabled.
            if (Kernel_Season.Instance.opt_huntEnabled)
            {
                _subGods.Add(new SubGod_Season_Hunt(map));
            }

            if (Kernel_Season.Instance.opt_windEnabled)
            {
                _subGods.Add(new SubGod_Season_Wind(map));
            }



            // Populate and shuffle the deck if the deck mode is in use.
            if (Kernel_Season.Instance.opt_deckMode)
            {
                CheckShuffleSubGodDeck();
            }
        }

        public override string getName()
        {
            if (_activeSubGod != null)
            {
                return "God of Seasons: " + _activeSubGod.GetName();
            }

            return "God of Seasons";
        }

        public override string getDescFlavour()
        {
            return "Placeholder god description.";
        }

        public override string getDescMechanics()
        {
            return "This Elder God is made up of different Fey Nobles who each take turns ruling the Court. When the season shifts, their playstyle changes radically - each noble has their own power set and playstyle, and victory will require shifting smoothly from one to the other.";
        }

        public override string getDetailedMechanics()
        {

            return "This Elder God plays by putting decisive bursts of resources toward a specific goal, then pivoting wildly toward different plans as they enjoy the last season's successes.\n\n<b>The Court</b>\nThe game begins with a random Noble in play with their own list of powers. Every " + Kernel_Season.Instance.opt_seasonLength + " turns, control of the Court will shift to a different random Noble, removing the previous Noble's power list and replacing it with a new one. This changing of the seasons also grants you a bonus based on the new ruler, allowing them to get off the ground quickly. You can use the Seize the Throne power to switch before time runs out, which also allows you to choose the next Noble to control, though the new Noble will not benefit from their normal Season Changes effect. The change can also be delayed using the Cling to the Throne power, but not indefinitely.\n\n<b>Fey Presence</b>\nThe Court's more impactful powers are fueled by the Fey Presence modifier. Each Noble has their own ways of generating Fey Presence in line with their playstyle, but the resource itself remains across seasons and can be used by all Nobles equally. If you have no plans left for your current Noble, consider spreading additional Fey Presence until the seasons change again.\n\n<b>The Supplicant</b>\nThe Supplicant does not occupy an agent slot, and takes different forms for different Nobles. Supplicants can outlive their Noble's season, but if the Supplicant is dead when the Nobles change rulership that Noble's Supplicant will emerge to serve you.";
        }

        public override void onStart(Map map)
        {
            turnsRemainingInSeason = Kernel_Season.Instance.opt_seasonLength;
            map.overmind.availableEnthrallments = 3;
            CheckShuffleSubGodDeck();

            ChangeActiveSubGod();
        }

        public override Sprite getSupplicant()
        {
            return map.world.textureStore.agent_supplicantSnake;
        }

        public override int[] getSealLevels()
        {
            return new int[9] { 12, 24, 44, 72, 108, 152, 204, 264, 375 };
        }

        public override int[] getAgentCaps()
        {
            return new int[10] { 1, 1, 2, 2, 3, 3, 3, 4, 4, 5 };
        }

        public override bool selectable()
        {
            return true;
        }

        public override int getMaxTurns()
        {
            return 500;
        }

        public override string getCredits()
        {
            return "Designed by the Shadows of Forbidden Gods Discord";
        }

        public override Sprite getGodPortrait(World world)
        {
            if (_activeSubGod != null && _activeSubGod.GetSpritePath() != "")
            {
                return EventManager.getImg(_activeSubGod.GetSpritePath());
            }

            return world.textureStore.god_snake_portrait;
        }

        public override Sprite getGodBackground(World world)
        {
            return EventManager.getImg("comseason.god_background.jpg");
        }

        public override double getWorldPanicOnAwake()
        {
            return 0.75;
        }

        public override void awaken()
        {
            base.awaken();
        }

        public override string getAwakenMessage()
        {
            if (_activeSubGod != null && _activeSubGod.GetAwakeningMessage() != "")
            {
                return _activeSubGod.GetAwakeningMessage();
            }

            return "Generic awakening message!";
        }

        public override string getVictoryMessage(int victoryMode)
        {
            if (_activeSubGod != null && _activeSubGod.GetVictoryMessage() != "")
            {
                return _activeSubGod.GetVictoryMessage();
            }

            switch (victoryMode)
            {
                case 0:
                    return "Generic shadow win!";
                case 1:
                    return "Generic madness win!";
                case 2:
                    return "Generic Dark Empire win!";
                case 3:
                    return "Generic destruction win!";
                case 4:
                    return "Generic ice age win!";
                case 5:
                    return "Generic Deep Ones win!";
                default:
                    return "Generic win from a strange VP source!";
            };
        }

        public bool CheckShuffleSubGodDeck()
        {
            if (_subGodDeck.Count == 0)
            {
                _subGodDeck.AddRange(_subGods);

                if (_activeSubGod != null)
                {
                    _subGodDeck.Remove(_activeSubGod);
                }

                // Implements Fisher–Yates shuffle algorithm
                for (int i = _subGodDeck.Count - 1; i > 1; i--)
                {
                    SubGod seasonA = _subGodDeck[i];
                    int index = Eleven.random.Next(i + 1);
                    if (index == i)
                    {
                        continue;
                    }

                    _subGodDeck[i] = _subGodDeck[index];
                    _subGodDeck[index] = seasonA;
                }

                return true;
            }

            return false;
        }

        public void removeSubGodPowers()
        {
            for (int i = 0; i < map.overmind.god.powers.Count; i++)
            {
                if (map.overmind.god.powers[i] is P_Season_SubGodPower)
                {
                    map.overmind.god.powers.RemoveAt(i);
                    map.overmind.god.powerLevelReqs.RemoveAt(i);
                    i--;
                }
            }
        }

        public void addSubGodPowers(SubGod toAdd, bool includeBonusPowers)
        {
            if (toAdd != null)
            {
                if (includeBonusPowers)
                {
                    for (int i = 0; i < toAdd.bonusPowers.Count; i++)
                    {
                        map.overmind.god.powers.Add(toAdd.bonusPowers[i]);
                        map.overmind.god.powerLevelReqs.Add(toAdd.bonusPowerLevelReqs[i]);
                    }
                }

                for (int i = 0; i < toAdd.powers.Count; i++)
                {
                    map.overmind.god.powers.Add(toAdd.powers[i]);
                    map.overmind.god.powerLevelReqs.Add(toAdd.powerLevelReqs[i]);
                }
            }
        }

        public void ChangeActiveSubGod(SubGod newSubGod = null, bool timedTransition = true)
        {
            if (newSubGod == null)
            {
                if (Kernel_Season.Instance.opt_deckMode)
                {
                    CheckShuffleSubGodDeck();

                    int lastIndex = _subGodDeck.Count - 1;
                    if (lastIndex < 0)
                    {
                        Console.WriteLine("ComSeasonGod: ERROR: No new sub god available.");
                    }

                    newSubGod = _subGodDeck[lastIndex]; // Gets the last subgod from the pre-shuffled deck.
                    _subGodDeck.RemoveAt(lastIndex);

                    CheckShuffleSubGodDeck();
                }
                else
                {
                    // A streaming-based algorithm that gives a totally random result
                    int i = 1;
                    foreach (SubGod season in _subGods)
                    {
                        if (season == _activeSubGod)
                        {
                            continue; // We can skip the current sub god without needing to duplicate the list, which is why I (ilikegoodfood) chose this algorithm.
                        }

                        if (Eleven.random.Next(i) == 0)
                        {
                            newSubGod = season;
                        }

                        i++;
                    }
                }
            }

            if (newSubGod == null)
            {
                Console.WriteLine("ComSeasonGod: ERROR: No new subGod available.");
                return;
            }

            lastTransitionWasTimed = timedTransition;

            if (_activeSubGod != null)
            {
                _activeSubGod.OnDeactivate(map, newSubGod, timedTransition);
            }

            // a Much fatser and cleaner method for managing the powers and power level reqs lists.
            // Assumes proper count matching.
            powers.Clear();
            powers.AddRange(_genericPowers);
            powers.AddRange(newSubGod.powers);

            powerLevelReqs.Clear();
            powerLevelReqs.AddRange(_genericPowerLevelReqs);
            powerLevelReqs.AddRange(newSubGod.powerLevelReqs);

            if (timedTransition)
            {
                powers.AddRange(newSubGod.bonusPowers);
                powerLevelReqs.AddRange(newSubGod.bonusPowerLevelReqs);
            }

            SubGod lastSubGod = _activeSubGod;
            _activeSubGod = newSubGod;
            newSubGod.OnActivate(map, _activeSubGod, timedTransition);

            foreach (SubGod subGod in _subGods)
            {
                subGod.OnSubGodTransition(map, lastSubGod, _activeSubGod, timedTransition);
            }

            if (!timedTransition || _activeSubGod.GetEventPathBonus() == "")
            {
                if (_activeSubGod.GetEventPath() != "")
                {
                    if (EventManager.events.ContainsKey(_activeSubGod.GetEventPath()))
                    {
                        EventContext ctx = EventContext.withNothing(map);
                        ctx.map.world.prefabStore.popEvent(EventManager.events[_activeSubGod.GetEventPath()].data, ctx, null, true);
                    }
                    else
                    {
                        Console.WriteLine("ComSeasonGod: ERROR: Season transition event is not loaded: " + _activeSubGod.GetEventPath());
                    }
                }
                else
                {
                    Console.WriteLine("ComSeasonGod: Season transition event is not defined.");
                    // Insert some geenric placeholder here.
                }
            }
            else
            {
                if (EventManager.events.ContainsKey(_activeSubGod.GetEventPathBonus()))
                {
                    EventContext ctx = EventContext.withNothing(map);
                    ctx.map.world.prefabStore.popEvent(EventManager.events[_activeSubGod.GetEventPathBonus()].data, ctx, null, true);
                }
                else
                {
                    Console.WriteLine("ComSeasonGod: ERROR: Season transition event is not loaded: " + _activeSubGod.GetEventPath());
                }
            }

            turnsRemainingInSeason = Kernel_Season.Instance.opt_seasonLength;
        }

        public override void turnTick()
        {
            base.turnTick();

            if (map.turn <= 0)
            {
                return;
            }

            turnsRemainingInSeason--;
            if (turnsRemainingInSeason <= 0)
            {
                ChangeActiveSubGod();
                turnsRemainingInSeason = Kernel_Season.Instance.opt_seasonLength;
            }

            foreach (SubGod subGod in _subGods)
            {
                if (subGod != _activeSubGod)
                {
                    subGod.TurnTick_Inactive(map);
                }
                else
                {
                    subGod.TurnTick_Active(map);
                }
            }

        }
    }
}
