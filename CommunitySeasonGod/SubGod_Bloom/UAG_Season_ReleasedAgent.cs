using Assets.Code;
using Assets.Code.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class UAG_Season_ReleasedAgent : UAG
    {
        public UA parentUnit;

        public UAG_Season_ReleasedAgent(UA parentUnit) : base(parentUnit.location, parentUnit.person.society, parentUnit.person)
        {
            this.parentUnit = parentUnit;
        }

        public override void turnTickInner(Map map)
        {
            base.turnTickInner(map);

            T_Season_SuppressedTrait.replaceTraits(person);
        }

        public override bool definesName()
        {
            return true;
        }

        public override string getName()
        {
            return "The Rebellious Supplicant";
        }

        public override Sprite getPortraitForeground()
        {
            return EventManager.getImg("ComSeasonGod.unit_supplicant_bloom.png");
        }

        public override Sprite getPortraitForegroundAlt()
        {
            return parentUnit.getPortraitForegroundAlt();
        }

        public static UA replaceWithHero(UA ua)
        {

            ua.corrupted = false;
            ua.map.units.Remove(ua);
            ua.location.units.Remove(ua);

            if (ua.task is Task_PerformChallenge challengeTask)
            {
                challengeTask.challenge.claimedBy = null;
            }

            UAG_Season_ReleasedAgent hero = new UAG_Season_ReleasedAgent(ua);

            hero.hp = ua.hp;
            hero.maxHp = ua.maxHp;
            hero.minions[0] = ua.minions[0];
            hero.minions[1] = ua.minions[1];
            hero.minions[2] = ua.minions[2];
            hero.inner_profile = ua.inner_profile;
            hero.inner_profileMin = ua.inner_profileMin;
            hero.inner_menace = ua.inner_menace;
            hero.inner_menaceMin = ua.inner_menaceMin;
            hero.movesTaken = ua.movesTaken;

            ua.person.unit = hero;
            ua.location.units.Add(hero);
            hero.map.units.Add(hero);
            T_Season_SuppressedTrait.replaceTraits(ua.person);

            foreach (Unit u in ua.map.units)
            {
                if (u.task is Task_AttackUnit attack && attack.target == ua)
                {
                    attack.target = hero;
                }
                else if (u.task is Task_AttackUnitWithEscort attack2 && attack2.target == ua)
                {
                    attack2.target = hero;
                }
                else if (u.task is Task_DisruptUA disrupt && disrupt.other == ua)
                {
                    disrupt.other = hero;
                }
                else if (u.task is Task_Bodyguard guard && guard.target == ua)
                {
                    guard.target = hero;
                }
            }


            foreach (SocialGroup soc in ua.map.socialGroups)
            {
                if (soc is HolyOrder order)
                {
                    if (order.prophet == ua)
                    {
                        order.prophet = hero;
                    }
                }
            }


            return ua;
        }

        public static UA replaceWithHero_OffMap(UA ua, UA baseForm)
        {
            UAG_Season_ReleasedAgent hero = new UAG_Season_ReleasedAgent(baseForm);

            hero.hp = baseForm.hp;
            hero.maxHp = baseForm.maxHp;
            hero.minions[0] = baseForm.minions[0];
            hero.minions[1] = baseForm.minions[1];
            hero.minions[2] = baseForm.minions[2];
            hero.inner_profile = baseForm.inner_profile;
            hero.inner_profileMin = baseForm.inner_profileMin;
            hero.inner_menace = baseForm.inner_menace;
            hero.inner_menaceMin = baseForm.inner_menaceMin;
            hero.movesTaken = baseForm.movesTaken;

            if (Kernel_Season.livingWilds?.GetType("LivingWilds.Kernel_Nature") != null)
            {
                Type nature = Kernel_Season.livingWilds.GetType("LivingWilds.Kernel_Nature");
                MethodInfo staticMethodInfo = nature.GetMethod("setNonWerewolfForm");
                staticMethodInfo.Invoke(null, new object[] { ua, hero });
            }

            return null;
        }

        public static UA replaceWithAgent(UAG_Season_ReleasedAgent released)
        {

            if (released.task is Task_PerformChallenge challengeTask)
                challengeTask.challenge.claimedBy = null;
            released.task = null;

            released.map.units.Remove(released);
            released.location.units.Remove(released);

            released.person.unit = released.parentUnit;
            released.map.units.Add(released.parentUnit);
            released.location.units.Add(released.parentUnit);

            released.parentUnit.location = released.location;
            released.parentUnit.hp = released.hp;
            released.parentUnit.maxHp = released.maxHp;
            released.parentUnit.minions[0] = released.minions[0];
            released.parentUnit.minions[1] = released.minions[1];
            released.parentUnit.minions[2] = released.minions[2];
            released.parentUnit.inner_profile = released.inner_profile;
            released.parentUnit.inner_profileMin = released.inner_profileMin;
            released.parentUnit.inner_menace = released.inner_menace;
            released.parentUnit.inner_menaceMin = released.inner_menaceMin;
            released.parentUnit.movesTaken = released.movesTaken;

            released.parentUnit.corrupted = true;

            foreach (Unit u in released.map.units)
            {
                if (u.task is Task_AttackUnit attack && attack.target == released)
                {
                    attack.target = released.parentUnit;
                }
                else if (u.task is Task_AttackUnitWithEscort attack2 && attack2.target == released)
                {
                    attack2.target = released.parentUnit;
                }
                else if (u.task is Task_DisruptUA disrupt && disrupt.other == released)
                {
                    disrupt.other = released.parentUnit;
                }
                else if (u.task is Task_Bodyguard guard && guard.target == released)
                {
                    guard.target = released.parentUnit;
                }
            }


            foreach (SocialGroup soc in released.map.socialGroups)
            {
                if (soc is HolyOrder order)
                {
                    if (order.prophet == released)
                    {
                        order.prophet = released.parentUnit;
                    }
                }
            }


            GraphicalMap.selectedUnit = released.parentUnit;
            return released.parentUnit;
        }

        public static UA replaceWithAgent_OffMap(UAG_Season_ReleasedAgent released)
        {

            if (released.task is Task_PerformChallenge challengeTask)
                challengeTask.challenge.claimedBy = null;
            released.task = null;

            released.parentUnit.location = released.location;
            released.parentUnit.hp = released.hp;
            released.parentUnit.maxHp = released.maxHp;
            released.parentUnit.minions[0] = released.minions[0];
            released.parentUnit.minions[1] = released.minions[1];
            released.parentUnit.minions[2] = released.minions[2];
            released.parentUnit.inner_profile = released.inner_profile;
            released.parentUnit.inner_profileMin = released.inner_profileMin;
            released.parentUnit.inner_menace = released.inner_menace;
            released.parentUnit.inner_menaceMin = released.inner_menaceMin;
            released.parentUnit.movesTaken = released.movesTaken;

            released.parentUnit.corrupted = true;

            if (Kernel_Season.livingWilds?.GetType("LivingWilds.Kernel_Nature") != null)
            {
                Type nature = Kernel_Season.livingWilds.GetType("LivingWilds.Kernel_Nature");
                MethodInfo staticMethodInfo = nature.GetMethod("setNonWerewolfForm");
                staticMethodInfo.Invoke(null, new object[] { released.person.unit, released.parentUnit });
            }

            return released.parentUnit;
        }



    }
}
