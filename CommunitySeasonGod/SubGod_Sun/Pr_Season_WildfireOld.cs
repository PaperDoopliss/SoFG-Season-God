using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{

    //DLC: UPDATE SO IT CAN'T SPREAD UNDERGROUND
    public class Pr_Season_WildfireOld : Property
    {

        public List<Challenge> endangeredChallenges = new List<Challenge>();
        public static int dangerToAdd = 5;

        public Pr_Season_WildfireOld(Location loc) : base(loc)
        {
            charge = 300;
        }

        public override string getName()
        {
            return "Wildfire";
        }

        public override void turnTick()
        {

            //If we've burned through all the Fey Presence, abort
            Pr_FeyPresence presence = null;
            Pr_Devastation devastation = null;
            Pr_Season_AshenEarth ash = null;

            foreach (Property pr in location.properties)
            {
                if (pr is Pr_FeyPresence foundPresence)
                    presence = foundPresence;
                else if (pr is Pr_Devastation foundDevastation)
                    devastation = foundDevastation;
                else if (pr is Pr_Season_AshenEarth foundAsh)
                    ash = foundAsh;
            }

            if (presence == null || presence.charge <= 0 || location.isOcean)
            {
                foreach (Challenge challenge in endangeredChallenges)
                {
                    challenge.addedDanger -= dangerToAdd;
                }
                location.properties.Remove(this);
                return;
            }

            //Spread to neighbours with Fey Presence
            foreach (Location l in location.getNeighbours())
            {
                if (l.isOcean)
                    continue;

                bool createWildfire = false;
                foreach (Property pr in l.properties)
                {
                    if (pr is Pr_Season_WildfireOld)
                    {
                        createWildfire = false; ;
                        break;
                    }
                    else if (pr is Pr_FeyPresence)
                    {
                        createWildfire = true;
                    }
                }


                if (createWildfire)
                {
                    l.properties.Add(new Pr_Season_WildfireOld(l));
                }
            }

            //Burn Fey Presence to create other effects
            double toBurn = Math.Min(P_Season_WildfireOld.presenceBurnPerTurn, presence.charge);
            if (devastation != null)
                devastation.influences.Add(new ReasonMsg("Wildfire", toBurn * P_Season_WildfireOld.devastationPerPresence));
            else 
            {
                devastation = new Pr_Devastation(location);
                devastation.charge = toBurn * P_Season_WildfireOld.devastationPerPresence;
                location.properties.Add(devastation);
            }

            if (ash != null)
                ash.influences.Add(new ReasonMsg("Wildfire", toBurn * P_Season_WildfireOld.ashPerPresence));
            else
            {
                ash = new Pr_Season_AshenEarth(location);
                ash.charge = toBurn * P_Season_WildfireOld.ashPerPresence;
                location.properties.Add(ash);
            }

            presence.influences.Add(new ReasonMsg("Burned for Wildfire", -toBurn));


            //Manage challenges
            foreach (Challenge ch in location.GetChallenges())
            {
                if (endangeredChallenges.Contains(ch) == false)
                {
                    endangeredChallenges.Add(ch);
                    ch.addedDanger += dangerToAdd;
                }
            }
        }
        

        public override bool hasHexView()
        {
            return true;
        }

        public override Sprite getSprite(World world)
        {
            return EventManager.getImg("ComSeasonGod.power_wildfire.png");
        }

        public override Sprite hexViewSprite()
        {
            return EventManager.getImg("ComSeasonGod.property_wildfire_hex.png");

        }

        public override string getDesc()
        {
            return "This location is suffering from a devastating wildfire. Every turn, up to " + P_Season_WildfireOld.presenceBurnPerTurn + "% Fey Presence will be consumed to create equal amounts of <b>devastation</b> and Ashen Earth. The Widlfire will spread to neighbouring locations with Fey Presence, and will fade when there's no Fey Presence left to consume. Challenges and quests in this location gain " + dangerToAdd + " <b>danger</b> while the Wildfire is active.";
        }

    }
}

