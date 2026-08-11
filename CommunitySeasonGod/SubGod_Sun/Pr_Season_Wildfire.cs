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

    public class Pr_Season_Wildfire : Property
    {

        public static int startingCooldown = 8;
        public int cooldown = startingCooldown;

        public static double presenceCutoff = 0;
        public static double devastationCutoff = 80;

        public Pr_FeyPresence fuelSource = null;

        public static double spreadCutoff = 200;
        public bool hasSpread = false;

        public static double burnoutCutoff = 100;
        public static double increasePerTurn = 15;
        public static double burnoutPerTurn = 10;
        public static double presenceBurnedPerTurn = 10;

        public Ch_Season_DouseFlames ch_douse = null;

        public Pr_Season_Wildfire(Location loc) : base(loc)
        {
            ch_douse = new Ch_Season_DouseFlames(loc, this);
        }

        public override string getInvariantName()
        {
            return "Wildfire";
        }

        public override string getName()
        {
            if (fuelSource != null)
                return "Bolstered Wildfire";
            if (cooldown > 0)
                return "Wildfire";
            return "Dying Wildfire";
        }

        public override List<Challenge> getChallenges()
        {
            return new List<Challenge> { ch_douse };
        }

        public override void turnTick()
        {
            double devastationToAdd = increasePerTurn;

            if (charge >= 300)
            {
                charge = 300;
                if (cooldown > 0)
                    cooldown--;
            }
            else if (charge <= 0)
                location.properties.Remove(this);
            else if (charge < burnoutCutoff && cooldown > 0)
                cooldown--;


            if (fuelSource != null && (fuelSource.charge <= 0 || location.properties.Contains(fuelSource) == false))
                fuelSource = null;
            if (fuelSource == null)
            {
                foreach (Property pr in location.properties)
                {
                    if (pr is Pr_FeyPresence presence && presence.charge >= presenceCutoff)
                        fuelSource = presence;
                }
            }

            if (fuelSource != null)
            {
                influences.Add(new ReasonMsg("Burning Fey Presence", increasePerTurn));
                devastationToAdd += increasePerTurn;

                double presenceToBurn = Math.Min(presenceBurnedPerTurn,fuelSource.charge);
                fuelSource.charge -= presenceToBurn;
                if (fuelSource.charge <= 0)
                    location.properties.Remove(fuelSource);

                Pr_Season_AshenEarth ash = null;
                foreach (Property pr in location.properties)
                {
                    if (pr is Pr_Season_AshenEarth foundAsh)
                    {
                        ash = foundAsh;
                        break;
                    }
                }

                if (ash == null)
                {
                    ash = new Pr_Season_AshenEarth(location);
                    ash.charge = presenceToBurn;
                    location.properties.Add(ash);
                }
                else
                    ash.influences.Add(new ReasonMsg("Burned Fey Presence", presenceToBurn));
            }
            if (cooldown > 0)
                influences.Add(new ReasonMsg("Spreading Flames", increasePerTurn));
            else
                influences.Add(new ReasonMsg("Dying Flames", -burnoutPerTurn));

            if (charge >= devastationCutoff)
            {
                Pr_Devastation devastation = null;
                foreach (Property pr in location.properties)
                {
                    if (pr is Pr_Devastation foundDevastation)
                    {
                        devastation = foundDevastation;
                        break;
                    }
                }

                if (devastation != null)
                {
                    devastationToAdd = Math.Min(devastationToAdd, charge - devastation.charge);
                    if (devastationToAdd > 0)
                        devastation.charge += devastationToAdd;
                }
                else
                {
                    devastation = new Pr_Devastation(location);
                    devastation.charge = devastationToAdd;
                    location.properties.Add(devastation);
                }

            }


            if (charge >= spreadCutoff && hasSpread == false)
            {
                hasSpread = true;

                List<Location> bestTargets = new List<Location>();
                int bestNumberOfWildfires = 0;

                foreach (Location l in location.getNeighbours())
                {
                    if (l.hex.isForest == false && l.hex.terrain != Hex.terrainType.JUNGLE && l.hex.terrain != Hex.terrainType.PLAINS && l.hex.terrain != Hex.terrainType.HIGHLAND && l.hex.terrain != Hex.terrainType.GRASS)
                        continue;

                    int numberOfWildfires = 0;
                    bool alreadyWildfire = false;
                    foreach (Property pr in l.properties)
                    {
                        if (pr is Pr_Season_Wildfire)
                        {
                            alreadyWildfire = true;
                            break;
                        }
                    }

                    if (alreadyWildfire)
                        continue;

                    foreach (Location l2 in l.getNeighbours())
                    {
                        foreach (Property pr in l2.properties)
                        {
                            if (pr is Pr_Season_Wildfire wildfire)
                            {
                                numberOfWildfires++;
                            }
                        }
                    }

                    if (numberOfWildfires > bestNumberOfWildfires)
                    {
                        bestNumberOfWildfires = numberOfWildfires;
                        bestTargets.Clear();
                    }
                    if (numberOfWildfires >= bestNumberOfWildfires)
                    {
                        bestTargets.Add(l);
                    }
                }

                if (bestTargets.Count > 0) 
                {
                    Location newFireLoc = bestTargets[Eleven.random.Next(bestTargets.Count)];
                    Pr_Season_Wildfire newWildfire = new Pr_Season_Wildfire(newFireLoc);
                    newWildfire.charge = 20;
                    newFireLoc.properties.Add(newWildfire);
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
            if (cooldown <= 0)
                return "This location is suffering from a devastating wildfire that is slowly dying out. Every turn, the wildfire's charge decreases by " + burnoutPerTurn + "%. If the Wildfire has 80 charge or more, Devastation will increase by up to " + increasePerTurn + "%, to a maximum of the Wildfire's charge. If there is Fey Presence at the location, the Wildfire will grow by " + (increasePerTurn - burnoutPerTurn) + "% instead of decreasing, and will convert " + Pr_Season_Wildfire.presenceBurnedPerTurn + "% Fey Presence a turn into Ashen Earth. Once in this Wildfire's lifetime, if its charge is at least " + spreadCutoff + ", it can spread to a neighbouring location that has a Plains, Grass, Jungle, Highland, or Forest on its hex. Heroes within 3 links will be motivated to fight the fire if its charge is at least 100%, and heroes within 7 links will be motivated to fight the fire if its charge is at least 200%. ";
            return "This location is suffering from a devastating wildfire. Every turn, its charge increases by " + Pr_Season_Wildfire.increasePerTurn + "%, to a maximum of 300%. If the Wildfire has " + devastationCutoff + "% charge or more, Devastation will increase by the same amount, to a maximum of the Wildfire's charge. If there is Fey Presence at the location, the Wildfire will double the speed of its growth and convert " + Pr_Season_Wildfire.presenceBurnedPerTurn + "% Fey Presence a turn into Ashen Earth. Once in this Wildfire's lifetime, if its charge is at least " + Pr_Season_Wildfire.spreadCutoff + ", it can spread to a neighbouring location that has a Plains, Grass, Jungle, Highland, or Forest on its hex. Heroes within 3 links will be motivated to fight the fire if its charge is at least 100%, and heroes within 7 links will be motivated to fight the fire if its charge is at least 200%. After " + cooldown + " more turns where the Wildfire's charge is below " + Pr_Season_Wildfire.burnoutCutoff + "% or at 300%, the Wildfire will begin dying down, decreasing by " + Pr_Season_Wildfire.burnoutPerTurn + "% instead of increasing.";
        }

    }
}

