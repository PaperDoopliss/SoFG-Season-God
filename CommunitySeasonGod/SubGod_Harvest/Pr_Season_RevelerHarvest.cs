using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Pr_Season_RevelerHarvest : Property
    {

        public static int reduceAmount = 5;
        public bool hasActivated = false;

        public Pr_Season_RevelerHarvest(Location loc)
            : base(loc)
        {
            charge = 50;
        }

        public override string getName()
        {
            return "Eternal Revelry";
        }

        public override string getDesc()
        {
            return "Sadness and anger dissipate as those here are enraptured by endless revelries. This modifier will consume Unrest and Lingering Resentment, immediately dissipating once there's none left or if ther season changes.\n\nThe local ruler will become disrupted while this is active (unless they are part of the Dark Empire). Unenshadowed heroes who enter this location have a chance of becoming disrupted. Your agents can reduce this modifier's charge to gather Mystic Bounty.";
        }

        public override Sprite getSprite(World world)
        {
            return EventManager.getImg("ComSeasonGod.power_blooming_fields.png");
        }

        public override bool removedOnRuin()
        {
            return true;
        }

        public override bool survivesRuin()
        {
            return false;
        }

        public override void turnTick()
        {
            base.turnTick();

            bool hasConsumable = false;
            bool isHarvestSeason = false;

            foreach (Property pr in location.properties)
            {
                if (pr is Pr_Unrest && hasActivated)
                {
                    pr.influences?.Add(new ReasonMsg("Eternal Revelry", -reduceAmount));
                    hasConsumable = true;
                }

                if (pr is Pr_LingeringResentment && hasActivated)
                {
                    pr.influences?.Add(new ReasonMsg("Eternal Revelry", -reduceAmount * 2));
                    hasConsumable = true;
                }
            }

            God_Season season = map.overmind.god as God_Season;
            if (season != null)
            {
                if (season.ActiveSubGod.GetName() == "Uncle of the Harvest")
                {
                    isHarvestSeason = true;
                }
            }

            if (!hasConsumable || !isHarvestSeason)
            {
                map.addMessage("The Eternal Revelry at " + location.getName() + " has ended!", 0.3, false, location.hex);
                location.properties.Remove(this);
            }

            if (!hasActivated)
            {
                return;
            }

            foreach (Unit u in location.units)
            {
                if (u != null)
                {
                    if (u.isCommandable())
                    {
                        continue;
                    }

                    if(u is UAG hero && !(hero.task is Task_Disrupted) && hero.person != null && hero.person.shadow < 1.0)
                    {
                        if(Eleven.random.Next(10) >= 6)
                        {
                            u.task = null;
                            u.task = new Task_Disrupted(5);
                            map.addMessage("A hero has been trapped by Eternal Revelry!", 0.3, true, location.hex);
                        }
                    }
                }
            }

            if (location != null && location.person() != null)
            {
                if (location.settlement != null && location.settlement is SettlementHuman human)
                {
                    if(location.soc != null && location.soc is Society society && !society.isDarkEmpire)
                    {
                        if (human.actionUnderway != null)
                        {
                            human.actionUnderway = null;
                            human.actionUnderway = new Act_Gridlock(location);
                        }
                    }
                }
            }



        }

    }
}
