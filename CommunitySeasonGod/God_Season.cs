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

        public Season_SubGod activeSubGod = null;
        public List<Season_SubGod> subGods = new List<Season_SubGod>();
        public List<Season_SubGod> pendingSubGods = new List<Season_SubGod>();

        public int turnsRemainingInSeason = 0;
        public bool hasClungToThrone = false;
        public bool lastShiftWasNatural = false;

        public override void setup(Map map)
        {
            base.setup(map);

            if (Kernel_Season.get().param_huntEnabled)
                subGods.Add(new SubGod_Season_Hunt(map));
            if (Kernel_Season.get().param_windEnabled)
                subGods.Add(new SubGod_Season_Wind(map));
        }

        public override string getName()
        {
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

            return "This Elder God plays by putting decisive bursts of resources toward a specific goal, then pivoting wildly toward different plans as they enjoy the last season's successes.\n\n<b>The Court</b>\nThe game begins with a random Noble in play with their own list of powers. Every " + Kernel_Season.get().seasonLength + " turns, control of the Court will shift to a different random Noble, removing the previous Noble's power list and replacing it with a new one. This changing of the seasons also grants you a bonus based on the new ruler, allowing them to get off the ground quickly. You can use the Seize the Throne power to switch before time runs out, which also allows you to choose the next Noble to control, though the new Noble will not benefit from their normal Season Changes effect. The change can also be delayed using the Cling to the Throne power, but not indefinitely.\n\n<b>Fey Presence</b>\nThe Court's more impactful powers are fueled by the Fey Presence modifier. Each Noble has their own ways of generating Fey Presence in line with their playstyle, but the resource itself remains across seasons and can be used by all Nobles equally. If you have no plans left for your current Noble, consider spreading additional Fey Presence until the seasons change again.\n\n<b>The Supplicant</b>\nThe Supplicant does not occupy an agent slot, and takes different forms for different Nobles. Supplicants can outlive their Noble's season, but if the Supplicant is dead when the Nobles change rulership that Noble's Supplicant will emerge to serve you.";
        }

        public override void onStart(Map map)
        {
            turnsRemainingInSeason = Kernel_Season.get().seasonLength;
            map.overmind.availableEnthrallments = 3;
            checkRefreshAvailableSubGods();

            changeSubGods();
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
            return new int[10] { 2, 2, 3, 3, 4, 4, 4, 5, 5, 6 };
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
            if (activeSubGod != null && activeSubGod.GetSpritePath() != "")
                return EventManager.getImg(activeSubGod.GetSpritePath());
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
            if (activeSubGod != null && activeSubGod.GetAwakeningMessage() != "")
                return activeSubGod.GetAwakeningMessage();
            return "Generic awakening message!";
        }

        public override string getVictoryMessage(int victoryMode)
        {
            if (activeSubGod != null && activeSubGod.GetVictoryMessage() != "")
                return activeSubGod.GetVictoryMessage();

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

        public void checkRefreshAvailableSubGods()
        {

            if (pendingSubGods.Count == 0 || pendingSubGods.Count == 1 && pendingSubGods[0] == activeSubGod)
            {

                pendingSubGods.Clear();

                for (int i = 0; i < subGods.Count; i++)
                {
                    pendingSubGods.Add(subGods[i]);
                }
            }
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

        public void addSubGodPowers(Season_SubGod toAdd, bool includeBonusPowers)
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

        public void changeSubGods(Season_SubGod newSubGod = null, bool transitionNaturally = true)
        {

            //Everyone on our Pending Sub God list except ourselves
            List<Season_SubGod> potentialSubGods = new List<Season_SubGod>();
            for (int i = 0; i < pendingSubGods.Count; i++)
            {
                if (pendingSubGods[i] != activeSubGod)
                    potentialSubGods.Add(pendingSubGods[i]);
            }

            if (newSubGod == null && potentialSubGods.Count > 0) 
                newSubGod = potentialSubGods[Eleven.random.Next(potentialSubGods.Count)];

            //Should always be true, but a sanity check
            if (newSubGod != null)
            {

                lastShiftWasNatural = transitionNaturally;

                foreach (Season_SubGod subGod in subGods)
                {
                    subGod.OnSubGodTransition(map, activeSubGod, newSubGod, transitionNaturally);
                }

                if (activeSubGod != null)
                {
                    activeSubGod.OnDeactivate(map, newSubGod, transitionNaturally);
                    removeSubGodPowers();
                }

                if (newSubGod != null)
                {
                    newSubGod.OnActivate(map, activeSubGod, transitionNaturally);
                    pendingSubGods.Remove(newSubGod);
                }

                activeSubGod = newSubGod;
                addSubGodPowers(newSubGod, transitionNaturally);


                if (!transitionNaturally || activeSubGod.GetEventPathBonus() == "")
                {
                    if (activeSubGod.GetEventPath() != "")
                    {
                        if (EventManager.events.ContainsKey(activeSubGod.GetEventPath()))
                        {
                            EventContext ctx = EventContext.withNothing(map);
                            ctx.map.world.prefabStore.popEvent(EventManager.events[activeSubGod.GetEventPath()].data, ctx, null, force: true);
                        }
                    }
                }
                else
                {
                    if (EventManager.events.ContainsKey(activeSubGod.GetEventPathBonus()))
                    {
                        EventContext ctx = EventContext.withNothing(map);
                        ctx.map.world.prefabStore.popEvent(EventManager.events[activeSubGod.GetEventPathBonus()].data, ctx, null, force: true);
                    }
                }

            }

            checkRefreshAvailableSubGods();

            turnsRemainingInSeason = Kernel_Season.get().seasonLength;
        }

        public override void turnTick()
        {
            base.turnTick();

            if (map.turn > 0)
            {
                turnsRemainingInSeason--;
                if (turnsRemainingInSeason <= 0)
                {

                    //Sanity check, we should always catch it after the subgod change instead of here
                    checkRefreshAvailableSubGods();

                    //Multiple SubGods in play
                    if (subGods.Count > 1)
                    {
                        changeSubGods();
                    }

                    else if (subGods.Count == 1) //For some reason there's only one SubGod in play
                    {
                        if (subGods[0] != activeSubGod) //If for some reason it's not the SubGod we currently have controlled
                        {
                            changeSubGods();
                        }
                        else
                            turnsRemainingInSeason = Kernel_Season.get().seasonLength;

                    }
                    else if (activeSubGod != null) //We have no possible SubGods, but somehow we still have an active one we need to clean up
                    {
                        removeSubGodPowers();
                        activeSubGod = null;
                        turnsRemainingInSeason = Kernel_Season.get().seasonLength;
                    }

                }
            }

            foreach (Season_SubGod subGod in subGods)
            {
                if (subGod != activeSubGod)
                    subGod.TurnTick_Inactive(map);
                else
                    subGod.TurnTick_Active(map);
            }

        }



    }
}
