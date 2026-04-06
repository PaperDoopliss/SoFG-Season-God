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
        protected List<SubGod> _draft = new List<SubGod>();

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
        protected List<Trait> _supplicantStartingTraits;

        [SerializeField]
        protected int _turnsRemainingInSeason = Kernel_Season.opt_seasonLength;
        public int TurnsRemainingInSeason => _turnsRemainingInSeason;

        [SerializeField]
        protected bool _stasisUsed = false;
        public bool StasisUsed { get; set; }

        [SerializeField]
        protected bool _lastShiftWasNatural = false;
        public bool LastShiftWasNatural => _lastShiftWasNatural;

        [SerializeField]
        protected bool _nextShiftIsNatural = true;
        public bool NextShiftIsNatural => _nextShiftIsNatural;

        public int ShiftPowerCost = -1;

        public override void setup(Map map)
        {
            base.setup(map);

            if (Kernel_Season.opt_windEnabled > 0)
            {
                SubGods.Add(new SubGod_Wind(this, map));
            }

            if (Kernel_Season.opt_harvestEnabled > 0)
            {
                SubGods.Add(new SubGod_Harvest(this, map));
            }

            if (Kernel_Season.opt_feastEnabled > 0)
            {
                SubGods.Add(new SubGod_Feast(this, map));
            }

            if (Kernel_Season.opt_bloomEnabled > 0)
            {
                SubGods.Add(new SubGod_Bloom(this, map));
            }

            _genericPowers.Add(new P_Stasis(map));
            _genericPowerLevelReqs.Add(2);
            _genericPowers.Add(new P_HostileShift(map));
            _genericPowerLevelReqs.Add(2);
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
            return "This Elder God is made up of different Fey Nobles who each take turns ruling the Court. When the season shifts, their playstyle changes radically - each Noble has their own power set and playstyle, and victory will require shifting smoothly from one to the other.";
        }

        public override string getDetailedMechanics()
        {

            return "This Elder God plays by putting decisive bursts of resources toward a specific goal, then pivoting wildly toward different plans as they enjoy the last season's successes.\n\n<b>The Court</b>\nThe game begins with one of three random Nobles in play with their own list of powers. Every " + Kernel_Season.opt_seasonLength + " turns, control of the Court will shift to a different selection of three Nobles, removing the previous Noble's power list and replacing it with a new one. This changing of the seasons also grants you a bonus based on the new ruler, allowing them to get off the ground quickly. You can use the Hostile Shift power to switch before time runs out, which also allows you to choose the next Noble to control, though the new Noble will not benefit from their normal Season Changes effect. The change can also be delayed using the Stasis power, but not indefinitely.\n\n<b>Fey Presence</b>\nThe Court's more impactful powers are fueled by the Fey Presence modifier. Each Noble has their own ways of generating Fey Presence in line with their playstyle, but the resource itself remains across seasons and can be used by all Nobles equally. If you have no plans left for your current Noble, consider spreading additional Fey Presence until the seasons change again.\n\n<b>The Supplicant</b>\nThe Supplicant does not occupy an agent slot, and takes different forms for different Nobles. Supplicants can outlive their Noble's season, but if the Supplicant is dead when the Nobles change rulership that Noble's Supplicant will emerge to serve you.";
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

            Kernel_Season.Instance.HasHostileShift = true;
            _turnsRemainingInSeason = Kernel_Season.opt_seasonLength;
            map.overmind.availableEnthrallments = 2;

            Unit supplicant = ElderTombLocation.units.FirstOrDefault(u => u is UAE_Supplicant);
            if (supplicant != null)
            {
                ElderTombLocation.units.Remove(supplicant);
                map.units.Remove(supplicant);
                map.overmind.agents.Remove(supplicant);
                GraphicalMap.selectedUnit = null;
            }

            _nextShiftIsNatural = true;
            CreateDraft(Array.Empty<SubGod>());
            FireSeasonChangeEvent();
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
            else
                _supplicantSprite = map.world.textureStore.agent_supplicantSnake;
        }

        public override bool hasSupplicantStartingTraits()
        {
            if (_supplicantStartingTraits != null && _supplicantStartingTraits.Count > 0)
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
            if (_supplicantStartingTraits == null || _supplicantStartingTraits.Count == 0)
            {
                FetchSupplcantStartingTraits();
                
            }

            return _supplicantStartingTraits;
        }

        private void FetchSupplcantStartingTraits()
        {
            if (ActiveSubGod != null && ActiveSubGod.HasSupplicantStartingTraits())
            {
                _supplicantStartingTraits = ActiveSubGod.GetSupplicantStartingTraits();
            }
            else
                _supplicantStartingTraits = new List<Trait>();
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
            return EventManager.getImg("ComSeasonGod.portrait_default.png");
        }

        public override Sprite getGodBackground(World world)
        {
            return EventManager.getImg("ComSeasonGod.god_background.jpg");
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
            if (ActiveSubGod != null && ActiveSubGod.GetVictoryMessage(victoryMode) != "")
                return ActiveSubGod.GetVictoryMessage(victoryMode);

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

        public bool TryApplyStasis()
        {
            if (StasisUsed)
            {
                for (int i = 0; i < powers.Count; i++)
                {
                    if (powers[i] is P_Stasis)
                    {
                        powers.RemoveAt(i);
                        powerLevelReqs.RemoveAt(i);
                    }
                }

                return false;
            }

            _turnsRemainingInSeason += Math.Max(10, Kernel_Season.opt_seasonLength / 2);
            StasisUsed = true;

            for (int i = 0; i < powers.Count; i++)
            {
                if (powers[i] is P_Stasis)
                {
                    powers.RemoveAt(i);
                    powerLevelReqs.RemoveAt(i);
                }
            }

            return true;

        }

        public void FireSeasonChangeEvent()
        {
            if (!EventManager.events.TryGetValue("ComSeasonGod.shift_choose", out EventManager.ActiveEvent ae))
            {
                Console.WriteLine("ComSeasonGod: Unable to find season change event (\"ComSeasonGod.shift_choose\").");
                map.addMessage("ERROR: Unable to find season change event (\"ComSeasonGod.shift_choose\").", 1.0, false);
                return;
            }

            map.world.prefabStore.popEvent(ae.data, EventContext.withNothing(map), null, false);
        }

        public void CreateDraft()
        {
            if (ActiveSubGod != null)
            {
                CreateDraft(new SubGod[1] { ActiveSubGod });
                return;
            }

            CreateDraft(Array.Empty<SubGod>());
        }

        public void CreateDraft(SubGod exclusion)
        {
            CreateDraft(new SubGod[1] { exclusion });
        }

        public void CreateDraft(IEnumerable<SubGod> exclusions)
        {
            int size;
            if (NextShiftIsNatural)
            {
                size = Kernel_Season.opt_draftSizeNatural;
            }
            else
            {
                size = Kernel_Season.opt_draftSizeSelection;
            }

            List<SubGod> subGods = new List<SubGod>();
            bool deckDraw = Kernel_Season.opt_deckMode;
            if (NextShiftIsNatural)
            {
                if (deckDraw)
                {
                    foreach (SubGod sub in _subGodDeck)
                    {
                        if (Kernel_Season.GetSubGodEnabledState(sub) == 3)
                        {
                            continue;
                        }

                        if (exclusions.Contains(sub))
                        {
                            continue;
                        }

                        subGods.Add(sub);
                    }

                    if (subGods.Count < size)
                    {
                        subGods.Clear();
                        ResetSubGodDeck();
                        deckDraw = false;
                    }
                }

                if (!deckDraw)
                {
                    foreach (SubGod subGod in SubGods)
                    {
                        int enableState = Kernel_Season.GetSubGodEnabledState(subGod);
                        if (enableState == 0 || enableState == 3)
                        {
                            continue;
                        }

                        if (exclusions.Contains(subGod))
                        {
                            continue;
                        }

                        subGods.Add(subGod);
                    }
                }
            }
            else
            {
                if (deckDraw)
                {
                    foreach (SubGod sub in _subGodDeck)
                    {
                        if (Kernel_Season.GetSubGodEnabledState(sub) == 2)
                        {
                            continue;
                        }

                        if (exclusions.Contains(sub))
                        {
                            continue;
                        }

                        subGods.Add(sub);
                    }

                    if (subGods.Count < size)
                    {
                        subGods.Clear();
                        ResetSubGodDeck();
                        deckDraw = false;
                    }
                }

                if (!deckDraw)
                {
                    foreach (SubGod sub in SubGods)
                    {
                        int enableState = Kernel_Season.GetSubGodEnabledState(sub);
                        if (enableState == 0 || enableState == 2)
                        {
                            continue;
                        }

                        if (exclusions.Contains(sub))
                        {
                            continue;
                        }

                        subGods.Add(sub);
                    }
                }
            }

            _draft.Clear();
            while (_draft.Count < size && subGods.Count > 0)
            {
                int index = Eleven.random.Next(subGods.Count);
                _draft.Add(subGods[index]);
                subGods.RemoveAt(index);
            }
        }

        public void ResetSubGodDeck()
        {
            _subGodDeck.Clear();
            foreach (SubGod subGod in SubGods)
            {
                int enableState = Kernel_Season.GetSubGodEnabledState(subGod);
                if (enableState == 0)
                {
                    continue;
                }

                _subGodDeck.Add(subGod);
            }
        }

        public void PresentDraft()
        {
            Sel2_SeasonSelector selector = new Sel2_SeasonSelector(this, _draft, ShiftPowerCost);
            List<string> targetLabels = new List<string> { "Random" };

            targetLabels.AddRange(_draft.Select<SubGod, string>(sg => sg.GetName() + " (" + sg.GetKeywords() + ")"));
            map.world.ui.addBlocker(map.world.prefabStore.getScrollSetText(targetLabels, false, selector, "Choose New Season", "Select the season to immediately transition to.").gameObject);
        }

        public void ChangeSubGodRandom(IEnumerable<SubGod> exclusions)
        {
            List<SubGod> subGods = new List<SubGod>();
            bool deckDraw = Kernel_Season.opt_deckMode;

            if (Kernel_Season.opt_deckMode)
            {
                ResetSubGodDeck();
            }

            if (NextShiftIsNatural)
            {
                foreach (SubGod subGod in SubGods)
                {
                    int enableState = Kernel_Season.GetSubGodEnabledState(subGod);
                    if (enableState == 0 || enableState == 3)
                    {
                        continue;
                    }

                    if (exclusions.Contains(subGod))
                    {
                        continue;
                    }

                    subGods.Add(subGod);
                }
            }
            else
            {
                foreach (SubGod subGod in SubGods)
                {
                    int enableState = Kernel_Season.GetSubGodEnabledState(subGod);
                    if (enableState == 0 || enableState == 2)
                    {
                        continue;
                    }

                    if (exclusions.Contains(subGod))
                    {
                        continue;
                    }

                    subGods.Add(subGod);
                }
            }

            if (subGods.Count == 0)
            {
                Console.WriteLine("ComSeasonGod: Unable to randomly switch sub-god: No new sub-god available.");
                map.addMessage("ERROR: No new sub god selected when trying to change sub god. The current sub god will be picked again.", 1.0, false);
            }
            else
            {
                ChangeSubGod(subGods[Eleven.random.Next(subGods.Count)]);
            }
                
        }

        public void ChangeSubGod(SubGod newSubGod)
        {
            if (Kernel_Season.opt_deckMode)
            {
                _subGodDeck.Remove(newSubGod);
            }

            if (newSubGod == null)
            {
                Console.WriteLine("ComSeasonGod: Unable to switch sub-god: No new sub-god available.");
                if (ActiveSubGod != null)
                {
                    map.addMessage("ERROR: No new sub god selected when trying to change sub god. The current sub god will be picked again.", 1.0, false);
                    newSubGod = ActiveSubGod;
                }
                else
                {
                    map.addMessage("ERROR: No new sub god selected when trying to change sub god.", 1.0, false);
                    Console.WriteLine("ComSeasonGod: ERROR: No active sub-god.");
                    return;
                }
            }

            _lastShiftWasNatural = NextShiftIsNatural;
            _nextShiftIsNatural = false;
            ShiftPowerCost = -1;
            StasisUsed = false;

            SubGod lastSubGod = ActiveSubGod;
            if (lastSubGod == null)
            {
                CreateDraft(new SubGod[1] { newSubGod });
            }
            else
            {
                CreateDraft(new SubGod[2] { newSubGod, lastSubGod });
                lastSubGod.OnDeactivate(map, newSubGod, _lastShiftWasNatural);
            }

            powers.Clear();
            powerLevelReqs.Clear();

            powers.AddRange(_genericPowers);
            powerLevelReqs.AddRange(_genericPowerLevelReqs);

            if (_lastShiftWasNatural)
            {
                powers.AddRange(_bonusGenericPowers);
                powerLevelReqs.AddRange(_bonusGenericPowerLevelReqs);
            }

            powers.AddRange(newSubGod.Powers);
            powerLevelReqs.AddRange(newSubGod.PowerLevelReqs);

            if (_lastShiftWasNatural)
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

            _activeSubGod = newSubGod;

            newSubGod.OnActivate(map, lastSubGod, _lastShiftWasNatural);

            foreach (SubGod subGod in SubGods)
            {
                subGod.OnSubGodTransition(map, lastSubGod, _activeSubGod, _lastShiftWasNatural);
            }

            CheckRespawnSupplicant();

            if (!_lastShiftWasNatural || ActiveSubGod.GetEventPathBonus() == "")
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

            _turnsRemainingInSeason = Kernel_Season.opt_seasonLength;
        }

        public override void turnTick()
        {
            base.turnTick();

            if (map.turn < 0)
            {
                return;
            }

            _turnsRemainingInSeason--;
            if (TurnsRemainingInSeason <= 0)
            {
                _nextShiftIsNatural = true;
                ShiftPowerCost = -1;
                CreateDraft(ActiveSubGod);
                FireSeasonChangeEvent();
            }

            foreach (SubGod subGod in SubGods)
            {
                if (subGod == ActiveSubGod)
                {
                    subGod.TurnTick_Active(map);
                    continue;
                }

                subGod.TurnTick_Inactive(map);
            }
        }

        public bool removeSubGod(Type toRemove)
        {
            if (_subGods.Count < 2)
                return false;

            for (int i = 0; i < _subGodDeck.Count; i++)
            {
                if (_subGodDeck[i].GetType() == toRemove)
                {
                    _subGodDeck.RemoveAt(i);
                    break;
                }
            }

            for (int i = 0; i < _subGods.Count; i++)
            {
                if (_subGods[i].GetType() == toRemove)
                {
                    _subGods.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public void forceNextShiftNatural(bool toForce)
        {
            _nextShiftIsNatural = toForce;
        }
    }
}
