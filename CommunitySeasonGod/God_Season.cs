using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class God_Season : God
    {

        [SerializeField]
        protected SubGod _activeSubGod = null;
        public SubGod ActiveSubGod => _activeSubGod;

        [SerializeField]
        protected List<SubGod> _subGods = new List<SubGod>();
        public List<SubGod> SubGods => _subGods;

        [SerializeField]
        protected List<SubGod> _subGodDeck = new List<SubGod>();

        [SerializeField]
        protected List<Power> _genericPowers = new List<Power>();

        [SerializeField]
        protected List<int> _genericPowerLevelReqs = new List<int>();

        [SerializeField]
        protected List<Power> _bonusGenericPowers = new List<Power>();

        [SerializeField]
        protected List<int> _bonusGenericPowerLevelReqs = new List<int>();

        [SerializeField]
        protected int _elderTombLocationIndex;
        public int ElderTombLocationIndex => _elderTombLocationIndex;

        public Location ElderTombLocation
        {
            get
            {
                if (ElderTombLocationIndex < 0 || ElderTombLocationIndex >= map.locations.Count)
                {
                    return null;
                }

                return map.locations[ElderTombLocationIndex];
            }
        }

        [SerializeField]
        protected Sprite _supplicantSprite;
        [SerializeField]
        protected List<Trait> _supplicantStartningTraits;

        public int turnsRemainingInSeason = Kernel_Season.opt_seasonLength;
        public bool hasClungToThrone = false;
        public bool lastShiftWasNatural = false;

        public override void setup(Map map)
        {
            base.setup(map);

            if (Kernel_Season.opt_huntEnabled > 0)
            {
                SubGods.Add(new SubGod_Hunt(this, map));
            }

            if (Kernel_Season.opt_windEnabled > 0)
            {
                SubGods.Add(new SubGod_Wind(this, map));
            }
        }

        public override string getName()
        {
            if (ActiveSubGod != null)
            {
                return "God of Seasons: " + ActiveSubGod.GetName();
            }

            return "God of Seasons";
        }

        public override string getDescFlavour()
        {
            return "Placeholder god description";
        }

        public override string getDescMechanics()
        {
            return "This Elder God is made up of different Fey Nobles who each take turns ruling the Court. When the season shifts, their playstyle changes radically - each noble has their own power set and playstyle, and victory will require shifting smoothly from one to the other.";
        }

        public override string getDetailedMechanics()
        {

            return "This Elder God plays by putting decisive bursts of resources toward a specific goal, then pivoting wildly toward different plans as they enjoy the last season's successes.\n\n<b>The Court</b>\nThe game begins with a random Noble in play with their own list of powers. Every " + Kernel_Season.opt_seasonLength + " turns, control of the Court will shift to a different random Noble, removing the previous Noble's power list and replacing it with a new one. This changing of the seasons also grants you a bonus based on the new ruler, allowing them to get off the ground quickly. You can use the Seize the Throne power to switch before time runs out, which also allows you to choose the next Noble to control, though the new Noble will not benefit from their normal Season Changes effect. The change can also be delayed using the Cling to the Throne power, but not indefinitely.\n\n<b>Fey Presence</b>\nThe Court's more impactful powers are fueled by the Fey Presence modifier. Each Noble has their own ways of generating Fey Presence in line with their playstyle, but the resource itself remains across seasons and can be used by all Nobles equally. If you have no plans left for your current Noble, consider spreading additional Fey Presence until the seasons change again.\n\n<b>The Supplicant</b>\nThe Supplicant does not occupy an agent slot, and takes different forms for different Nobles. Supplicants can outlive their Noble's season, but if the Supplicant is dead when the Nobles change rulership that Noble's Supplicant will emerge to serve you.";
        }

        public override void onStart(Map map)
        {
            base.onStart(map);
            foreach (Location loc in map.locations)
            {
                if (CommunityLib.ModCore.Get().checkIsElderTomb(loc))
                {
                    _elderTombLocationIndex = loc.index;
                    break;
                }
            }

            turnsRemainingInSeason = Kernel_Season.opt_seasonLength;
            map.overmind.availableEnthrallments = 2;
            if (Kernel_Season.opt_deckMode)
            {
                CheckShuffleSubGodDeck();
            }

            ChangeSubGods();
        }

        #region supplicant
        public override Sprite getSupplicant()
        {
            if (_supplicantSprite == null)
            {
                FetchSupplicantSprite();
            }

            return _supplicantSprite;
        }

        private void FetchSupplicantSprite()
        {
            if (ActiveSubGod != null)
            {
                _supplicantSprite = ActiveSubGod.GetSupplicantSprite();
            }

            _supplicantSprite = map.world.textureStore.agent_supplicantSnake;
        }

        public override bool hasSupplicantStartingTraits()
        {
            if (_supplicantStartningTraits != null && _supplicantStartningTraits.Count > 0)
            {
                return true;
            }

            if (ActiveSubGod != null)
            {
                return ActiveSubGod.HasSupplicantStartingTraits();
            }

            return false;
        }

        public override List<Trait> getSupplicantStartingTraits()
        {
            if (_supplicantStartningTraits == null || _supplicantStartningTraits.Count == 0)
            {
                FetchSupplcantStartingTraits();
                
            }

            return _supplicantStartningTraits;
        }

        private void FetchSupplcantStartingTraits()
        {
            if (ActiveSubGod != null && ActiveSubGod.HasSupplicantStartingTraits())
            {
                _supplicantStartningTraits = ActiveSubGod.GetSupplicantStartingTraits();
            }

            _supplicantStartningTraits = new List<Trait>();
        }

        public virtual bool CheckRespawnSupplicant()
        {
            if (map.overmind.agents.Any(u => u is UAE_Supplicant))
            {
                return false;
            }

            if (ElderTombLocation == null)
            {
                return false;
            }

            FetchSupplicantSprite();
            FetchSupplcantStartingTraits();
            RespawnSupplicant(map);
            return true;
        }

        public virtual void RespawnSupplicant(Map map)
        {
            UAE_Supplicant supplicant = new UAE_Supplicant(ElderTombLocation, map.soc_dark);
            map.units.Add(supplicant);
            ElderTombLocation.units.Add(supplicant);
            map.overmind.agents.Insert(0, supplicant);
        }
        #endregion

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
            if (ActiveSubGod != null && ActiveSubGod.GetSpritePath() != "")
            {
                return EventManager.getImg(ActiveSubGod.GetSpritePath());
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
            if (ActiveSubGod != null && ActiveSubGod.GetAwakeningMessage() != "")
            {
                return ActiveSubGod.GetAwakeningMessage();
            }
            return "Generic awakening message!";
        }

        public override string getVictoryMessage(int victoryMode)
        {
            if (ActiveSubGod != null && ActiveSubGod.GetVictoryMessage() != "")
                return ActiveSubGod.GetVictoryMessage();

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
            if (_subGodDeck.Count != 0)
            {
                return false;
            }

            if (SubGods.Count == 0 || (SubGods.Count == 1 & SubGods[0] == ActiveSubGod) || SubGods.All(sg => Kernel_Season.GetSubGodEnabledState(sg) == 0 || Kernel_Season.GetSubGodEnabledState(sg) == 3))
            {
                return false;
            }

            ShuffleSubGodDeck();
            return true;
        }

        public void ShuffleSubGodDeck()
        {
            _subGodDeck.Clear();
            
            foreach (SubGod season in SubGods)
            {
                int enabledState = Kernel_Season.GetSubGodEnabledState(season);
                if (enabledState == 0 || enabledState == 3)
                {
                    continue;
                }

                _subGodDeck.Add(season);
            }

            // Fisher–Yates shuffle - A simple linear O(n) shuffling algorithm with uniform results.
            for (int i = _subGodDeck.Count - 1; i > 0; i--)
            {
                int index = Eleven.random.Next(i + 1);
                if (index == i)
                {
                    continue;
                }

                SubGod season = _subGodDeck[index];
                _subGodDeck[index] = _subGodDeck[i];
                _subGodDeck[i] = season;
            }
        }

        public SubGod DrawSubGod()
        {
            CheckShuffleSubGodDeck();
            int lastIndex = _subGodDeck.Count - 1;
            SubGod season = _subGodDeck[lastIndex];
            _subGodDeck.RemoveAt(lastIndex);

            return season;
        }

        public SubGod SelectRandomSubGod()
        {
            // A linear streaming-based random selection algorithm that gives uniform results. Allows pre-processing of values without list duplication.
            SubGod result = null;
            int n = 1;
            foreach (SubGod season in SubGods)
            {
                if (season == ActiveSubGod)
                {
                    continue;
                }

                int enabledState = Kernel_Season.GetSubGodEnabledState(season);
                if (enabledState == 0 || enabledState == 3)
                {
                    continue;
                }

                if (Eleven.random.Next(n) == 0)
                {
                    result = season;
                }

                n++;
            }

            return result;
        }

        public SubGod PeekSubGodDeck()
        {
            CheckShuffleSubGodDeck();
            return _subGodDeck[_subGodDeck.Count - 1];
        }

        public void ChangeSubGods(SubGod newSubGod = null, bool transitionNaturally = true)
        {
            if (newSubGod == null)
            {
                if (Kernel_Season.opt_deckMode)
                {
                    newSubGod = DrawSubGod();
                }
                else
                {
                    newSubGod = SelectRandomSubGod();
                }
            }

            if (newSubGod == null)
            {
                Console.WriteLine("ComSeasonGod: Unable to switch sub-god: No new sub-god available.");
                if (ActiveSubGod != null)
                {
                    newSubGod = ActiveSubGod;
                }
                else
                {
                    Console.WriteLine("ComSeasonGod: ERROR: No active sub-god.");
                    return;
                }
            }

            lastShiftWasNatural = transitionNaturally;

            ActiveSubGod?.OnDeactivate(map, newSubGod, transitionNaturally);

            powers.Clear();
            powerLevelReqs.Clear();

            powers.AddRange(_genericPowers);
            powerLevelReqs.AddRange(_genericPowerLevelReqs);

            if (transitionNaturally)
            {
                powers.AddRange(_bonusGenericPowers);
                powerLevelReqs.AddRange(_bonusGenericPowerLevelReqs);
            }

            powers.AddRange(newSubGod.Powers);
            powerLevelReqs.AddRange(newSubGod.PowerLevelReqs);

            if (transitionNaturally)
            {
                powers.AddRange(newSubGod.BonusPowers);
                powerLevelReqs.AddRange(newSubGod.BonusPowerLevelReqs);
            }

            foreach (Power power in powers)
            {
                if (!(power is P_Season_LimitedCharges limitedPower))
                {
                    continue;
                }

                limitedPower.ResetCharges();
            }

            SubGod lastSeason = _activeSubGod;
            _activeSubGod = newSubGod;

            newSubGod.OnActivate(map, ActiveSubGod, transitionNaturally);

            foreach (SubGod subGod in SubGods)
            {
                subGod.OnSubGodTransition(map, lastSeason, _activeSubGod, transitionNaturally);
            }

            CheckRespawnSupplicant();

            if (!transitionNaturally || ActiveSubGod.GetEventPathBonus() == "")
            {
                if (ActiveSubGod.GetEventPath() != "")
                {
                    if (EventManager.events.ContainsKey(ActiveSubGod.GetEventPath()))
                    {
                        EventContext ctx = EventContext.withNothing(map);
                        ctx.map.world.prefabStore.popEvent(EventManager.events[ActiveSubGod.GetEventPath()].data, ctx, null, force: true);
                    }
                }
            }
            else
            {
                if (EventManager.events.ContainsKey(ActiveSubGod.GetEventPathBonus()))
                {
                    EventContext ctx = EventContext.withNothing(map);
                    ctx.map.world.prefabStore.popEvent(EventManager.events[ActiveSubGod.GetEventPathBonus()].data, ctx, null, force: true);
                }
            }

            turnsRemainingInSeason = Kernel_Season.opt_seasonLength;
        }

        public override void turnTick()
        {
            base.turnTick();

            if (map.turn < 0)
            {
                return;
            }

            turnsRemainingInSeason--;
            if (turnsRemainingInSeason <= 0)
            {
                ChangeSubGods();
            }

            foreach (SubGod subGod in SubGods)
            {
                if (subGod == ActiveSubGod)
                {
                    subGod.TurnTick_Active(map);
                }
                else
                {
                    subGod.TurnTick_Inactive(map);
                }
            }

        }
    }
}
