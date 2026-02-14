using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Pr_Season_WindCurrent : Property
    {

        public enum windCurrentDirection
        {
            NW,
            NE,
            SE,
            SW,
            All
        }

        public enum windCurrentEffect
        {
            NONE,
            CRISIS,
            SHADOW,
            POPULATION
        }

        public static double chargeDelta = 1;
        public static double chargeDeltaEmpowered = 3;

        public static double crisisPerTurn = 3;
        public static double crisisCostPerTurn = 3;

        public static double shadowPerTurn = 0.05;
        public static double shadowCostPerTurn = 3;

        public static int popPerTurn = 2;
        public static double popCostPerTurn = 3;

        public bool empowered = false;
        public List<Location> downwind;

        public windCurrentDirection direction;
        public windCurrentEffect effect = windCurrentEffect.NONE;

        public Pr_Season_WindCurrent(Location loc, windCurrentDirection direction)
            : base(loc)
        {
            this.direction = direction;
            updateDownwindLocations();
        }

        public override string getInvariantName()
        {
            return "Wind Current";
        }

        public override string getName()
        {
            string name = "";
            if (empowered)
                name += "Empowered ";
            else if (map.overmind.god is God_Season season && season.ActiveSubGod is SubGod_Wind == false)
                name += "Dormant ";

            if (effect == windCurrentEffect.CRISIS)
                name += "Tumultuous Current (";
            else if (effect == windCurrentEffect.SHADOW)
                name += "Smothering Current (";
            else if (effect == windCurrentEffect.POPULATION)
                name += "Mesmerizing Current (";
            else
                name += "Wind Current (";

            return name + direction + ")";
        }

        public void updateDownwindLocations(bool alsoNeighbours = true)
        {
            downwind = new List<Location>();
            //North hexes are lower than south hexes, east hexes are lower than west hexes
            foreach (Location l in location.getNeighbours())
            {

                if (direction != windCurrentDirection.All)
                {
                    if (locationWouldBeDownwind(l))
                        downwind.Add(l);
                }
                else
                {
                    Pr_Season_WindCurrent foundCurrent = null;
                    foreach (Property pr in l.properties)
                    {
                        if (pr is Pr_Season_WindCurrent current)
                        {
                            foundCurrent = current;
                            break;
                        }
                    }
                    if (foundCurrent == null)
                        downwind.Add(l);
                    else
                    {
                        if (foundCurrent.locationWouldBeDownwind(location) == false)
                            downwind.Add(l);
                    }

                }
            }

            if (alsoNeighbours)
            {
                foreach (Location l in location.getNeighbours())
                {
                    foreach (Property pr in l.properties)
                    {
                        if (pr is Pr_Season_WindCurrent current)
                        {
                            current.updateDownwindLocations(false);
                        }
                    }
                }
            }
        }

        public bool locationWouldBeDownwind(Location l)
        {
            if (direction == windCurrentDirection.All)
                return true;


            //Horizontals
            if (l.hex.y == location.hex.y && l.hex.x >= location.hex.x)
            {
                if (direction == windCurrentDirection.SW || direction == windCurrentDirection.NW)
                {
                    downwind.Add(l);
                }
            }
            else if (l.hex.y == location.hex.y && l.hex.x <= location.hex.x)
            {
                if (direction == windCurrentDirection.SE || direction == windCurrentDirection.NE)
                {
                    downwind.Add(l);
                }
            }
            else if (l.hex.y <= location.hex.y && l.hex.x == location.hex.x && l.hex.y % 2 == location.hex.y % 2)
            {
                if (direction == windCurrentDirection.NW || direction == windCurrentDirection.NE)
                {
                    downwind.Add(l);
                }
            }
            else if (l.hex.y >= location.hex.y && l.hex.x == location.hex.x && l.hex.y % 2 == location.hex.y % 2)
            {
                if (direction == windCurrentDirection.SW || direction == windCurrentDirection.SE)
                {
                    downwind.Add(l);
                }
            }

            //Diagonals (same X, but it's offset)
            else if (l.hex.y <= location.hex.y && l.hex.x == location.hex.x && l.hex.y % 2 != location.hex.y % 2)
            {
                if (direction == windCurrentDirection.NE)
                {
                    downwind.Add(l);
                }
            }
            else if (l.hex.y >= location.hex.y && l.hex.x == location.hex.x && l.hex.y % 2 != location.hex.y % 2)
            {
                if (direction == windCurrentDirection.SW)
                {
                    downwind.Add(l);
                }
            }

            //Diagonals
            else if (l.hex.y < location.hex.y && l.hex.x > location.hex.x)
            {
                if (direction == windCurrentDirection.NW)
                    downwind.Add(l);
            }
            else if (l.hex.y < location.hex.y && l.hex.x < location.hex.x)
            {
                if (direction == windCurrentDirection.NE)
                    downwind.Add(l);
            }
            else if (l.hex.y > location.hex.y && l.hex.x < location.hex.x)
            {
                if (direction == windCurrentDirection.SE)
                    downwind.Add(l);
            }
            else if (l.hex.y > location.hex.y && l.hex.x >= location.hex.x)
            {
                if (direction == windCurrentDirection.SW)
                    downwind.Add(l);
            }



            return false;
        }

        public override void turnTick()
        {

            Property localFeyPresence = null;
            foreach (Property pr in location.properties)
            {
                if (pr is Pr_FeyPresence)
                {
                    localFeyPresence = pr;
                }
            }

            //Grow Fey Presence here and nearby
            if (empowered || (map.overmind.god is God_Season season && season.ActiveSubGod is SubGod_Wind))
            {
                if (localFeyPresence != null)
                {
                    if (empowered)
                        localFeyPresence.influences.Add(new ReasonMsg("Empowered Wind Current", chargeDeltaEmpowered));
                    else
                        localFeyPresence.influences.Add(new ReasonMsg("Wind Current", chargeDelta));
                }
                else
                {
                    localFeyPresence = new Pr_FeyPresence(location);
                    if (empowered)
                        localFeyPresence.charge = chargeDeltaEmpowered;
                    else
                        localFeyPresence.charge = chargeDelta;
                    location.properties.Add(localFeyPresence);
                }

                foreach (Location l in downwind)
                {
                    bool foundPresence2 = false;
                    foreach (Property pr in l.properties)
                    {
                        if (pr is Pr_FeyPresence)
                        {
                            foundPresence2 = true;

                            if (empowered)
                                pr.influences.Add(new ReasonMsg("Empowered Wind Current", chargeDeltaEmpowered));
                            else
                                pr.influences.Add(new ReasonMsg("Wind Current", chargeDelta));
                        }
                    }
                    if (!foundPresence2)
                    {
                        Pr_FeyPresence presence = new Pr_FeyPresence(l);
                        if (empowered)
                            presence.charge = chargeDeltaEmpowered;
                        else
                            presence.charge = chargeDelta;
                        l.properties.Add(presence);
                    }
                }
            }

            if (effect == windCurrentEffect.CRISIS)
            {

                if (localFeyPresence?.charge >= crisisCostPerTurn)
                {

                    double localUnrest = 0;
                    double localPlague = 0;
                    double localDevastation = 0;
                    bool paid = false;

                    foreach (Property pr in location.properties)
                    {
                        if (pr is Pr_Unrest)
                            localUnrest += pr.charge;
                        else if (pr is Pr_Plague)
                            localPlague += pr.charge;
                        else if (pr is Pr_Devastation)
                            localDevastation += pr.charge;
                    }

                    foreach (Location l in downwind)
                    {
                        if (l.settlement is SettlementHuman == false)
                            continue;

                        Property foundUnrest = null;
                        Property foundPlague = null;
                        Property foundDevastation = null;
                        double targetUnrest = 0;
                        double targetPlague = 0;
                        double targetDevastation = 0;

                        foreach (Property pr in l.properties)
                        {
                            if (pr is Pr_Unrest)
                            {
                                foundUnrest = pr;
                                targetUnrest += pr.charge;
                            }
                            else if (pr is Pr_Plague)
                            {
                                foundPlague = pr;
                                targetPlague += pr.charge;
                            }
                            else if (pr is Pr_Devastation)
                            {
                                foundDevastation = pr;
                                targetDevastation += pr.charge;
                            }
                        }

                        if (targetUnrest < localUnrest || targetPlague < localPlague || targetDevastation < localDevastation)
                        {



                            if (paid == false)
                            {
                                localFeyPresence.influences.Add(new ReasonMsg("Spreading Crises", -crisisCostPerTurn));
                                paid = true;
                            }

                            if (targetUnrest < localUnrest)
                            {
                                if (foundUnrest == null)
                                {
                                    Pr_Unrest newUnrest = new Pr_Unrest(l);
                                    newUnrest.charge = Math.Min(crisisPerTurn, localUnrest);
                                    l.properties.Add(newUnrest);
                                }
                                else
                                    foundUnrest.influences.Add(new ReasonMsg("Tumultuous Current", Math.Min(crisisPerTurn, localUnrest - targetUnrest)));
                            }
                            if (targetPlague < localPlague)
                            {
                                if (foundPlague == null)
                                {
                                    Pr_Plague newPlague = new Pr_Plague(l);
                                    newPlague.charge = Math.Min(crisisPerTurn, localPlague);
                                    l.properties.Add(newPlague);
                                }
                                else
                                    foundPlague.influences.Add(new ReasonMsg("Tumultuous Current", Math.Min(crisisPerTurn, localPlague - targetPlague)));
                            }
                            if (targetDevastation < localDevastation)
                            {
                                if (foundDevastation == null)
                                {
                                    Pr_Devastation newDevastation = new Pr_Devastation(l);
                                    newDevastation.charge = Math.Min(crisisPerTurn, localDevastation);
                                    l.properties.Add(newDevastation);
                                }
                                else
                                    foundDevastation.influences.Add(new ReasonMsg("Tumultuous Current", Math.Min(crisisPerTurn, localDevastation - targetDevastation)));
                            }
                        }

                    }

                }

            }
            else if (effect == windCurrentEffect.SHADOW)
            {

                if (localFeyPresence?.charge >= shadowCostPerTurn)
                {

                    bool paid = false;

                    double localShadow = 1 - location.hex.purity;
                    if (location.settlement != null)
                        localShadow = location.settlement.shadow;

                    foreach (Location l in downwind)
                    {
                        double wardFraction = 0;

                        if (l.settlement != null && l.settlement.shadow < localShadow)
                        {

                            foreach (Property pr in l.properties)
                            {
                                if (pr is Pr_Ward)
                                {
                                    wardFraction += pr.charge / 100;
                                    if (wardFraction >= 1)
                                        continue;
                                }
                            }

                            if (!paid)
                            {
                                localFeyPresence.influences.Add(new ReasonMsg("Spreading Shadow", -shadowCostPerTurn));
                                paid = true;
                            }

                            double shadowChange = Math.Min(localShadow - l.settlement.shadow, shadowPerTurn);
                            shadowChange *= 1 - wardFraction;
                            l.settlement.shadow = Math.Min(1, l.settlement.shadow + shadowChange);
                        }
                    }

                }
            }
            else if (effect == windCurrentEffect.POPULATION && location.settlement is SettlementHuman sh)
            {

                if (localFeyPresence?.charge >= popCostPerTurn)
                {

                    bool paid = false;

                    foreach (Location l in downwind)
                    {
                        if (l.settlement is SettlementHuman sh2)
                        {

                            int popToMove = Math.Min(popPerTurn, sh.population);
                            if (sh2 is Set_MinorHuman)
                                popToMove = Math.Min(popToMove, 40 - sh2.population);

                            if (popToMove > 0)
                            {
                                if (!paid)
                                {
                                    localFeyPresence.influences.Add(new ReasonMsg("Drawing Population", -popCostPerTurn));
                                    paid = true;
                                }

                                sh.population -= popToMove;
                                sh2.population += popToMove;


                                foreach (Property pr in location.properties)
                                {
                                    if (pr is Pr_Season_IndustriousNewcomers)
                                    {
                                        pr.charge -= popToMove;
                                        break;
                                    }
                                }

                                bool propertyFoundInTarget = false;
                                foreach (Property pr in l.properties)
                                {
                                    if (pr is Pr_Season_IndustriousNewcomers)
                                    {
                                        pr.charge += popToMove;
                                        propertyFoundInTarget = true;
                                        break;
                                    }
                                }
                                if (!propertyFoundInTarget)
                                {
                                    Pr_Season_IndustriousNewcomers newcomers = new Pr_Season_IndustriousNewcomers(l);
                                    newcomers.charge = popToMove;
                                    l.properties.Add(newcomers);
                                }

                            }

                        }

                    }


                }
            }
            
        }

        public override bool hasHexView()
        {
            return true;
        }

        public override Sprite hexViewSprite()
        {
            if (effect == windCurrentEffect.CRISIS)
            {
                if (direction == windCurrentDirection.NW)
                    return EventManager.getImg("ComSeasonGod.property_current_nw_crisis_hex.png");
                else if (direction == windCurrentDirection.NE)
                    return EventManager.getImg("ComSeasonGod.property_current_ne_crisis_hex.png");
                else if (direction == windCurrentDirection.SE)
                    return EventManager.getImg("ComSeasonGod.property_current_se_crisis_hex.png");
                else if (direction == windCurrentDirection.SW)
                    return EventManager.getImg("ComSeasonGod.property_current_sw_crisis_hex.png");
                return EventManager.getImg("ComSeasonGod.property_current_generic_crisis_hex.png");
            }
            else if (effect == windCurrentEffect.SHADOW)
            {
                if (direction == windCurrentDirection.NW)
                    return EventManager.getImg("ComSeasonGod.property_current_nw_shadow_hex.png");
                else if (direction == windCurrentDirection.NE)
                    return EventManager.getImg("ComSeasonGod.property_current_ne_shadow_hex.png");
                else if (direction == windCurrentDirection.SE)
                    return EventManager.getImg("ComSeasonGod.property_current_se_shadow_hex.png");
                else if (direction == windCurrentDirection.SW)
                    return EventManager.getImg("ComSeasonGod.property_current_sw_shadow_hex.png");
                return EventManager.getImg("ComSeasonGod.property_current_generic_shadow_hex.png");
            }
            else if (effect == windCurrentEffect.POPULATION)
            {
                if (direction == windCurrentDirection.NW)
                    return EventManager.getImg("ComSeasonGod.property_current_nw_population_hex.png");
                else if (direction == windCurrentDirection.NE)
                    return EventManager.getImg("ComSeasonGod.property_current_ne_population_hex.png");
                else if (direction == windCurrentDirection.SE)
                    return EventManager.getImg("ComSeasonGod.property_current_se_population_hex.png");
                else if (direction == windCurrentDirection.SW)
                    return EventManager.getImg("ComSeasonGod.property_current_sw_population_hex.png");
                return EventManager.getImg("ComSeasonGod.property_current_generic_population_hex.png");
            }

            if (direction == windCurrentDirection.NW)
                return EventManager.getImg("ComSeasonGod.property_current_nw_hex.png");
            else if (direction == windCurrentDirection.NE)
                return EventManager.getImg("ComSeasonGod.property_current_ne_hex.png");
            else if (direction == windCurrentDirection.SE)
                return EventManager.getImg("ComSeasonGod.property_current_se_hex.png");
            else if (direction == windCurrentDirection.SW)
                return EventManager.getImg("ComSeasonGod.property_current_sw_hex.png");
            return EventManager.getImg("ComSeasonGod.property_current_generic_hex.png");
        }

        public override Sprite getSprite(World world)
        {
            if (effect == windCurrentEffect.CRISIS)
            {
                if (direction == windCurrentDirection.NW)
                    return EventManager.getImg("ComSeasonGod.property_current_nw_crisis.png");
                else if (direction == windCurrentDirection.NE)
                    return EventManager.getImg("ComSeasonGod.property_current_ne_crisis.png");
                else if (direction == windCurrentDirection.SE)
                    return EventManager.getImg("ComSeasonGod.property_current_se_crisis.png");
                else if (direction == windCurrentDirection.SW)
                    return EventManager.getImg("ComSeasonGod.property_current_sw_crisis.png");
                return EventManager.getImg("ComSeasonGod.property_current_generic_crisis.png");
            }
            else if (effect == windCurrentEffect.SHADOW)
            {
                if (direction == windCurrentDirection.NW)
                    return EventManager.getImg("ComSeasonGod.property_current_nw_shadow.png");
                else if (direction == windCurrentDirection.NE)
                    return EventManager.getImg("ComSeasonGod.property_current_ne_shadow.png");
                else if (direction == windCurrentDirection.SE)
                    return EventManager.getImg("ComSeasonGod.property_current_se_shadow.png");
                else if (direction == windCurrentDirection.SW)
                    return EventManager.getImg("ComSeasonGod.property_current_sw_shadow.png");
                return EventManager.getImg("ComSeasonGod.property_current_generic_shadow.png");
            }
            else if (effect == windCurrentEffect.POPULATION)
            {
                if (direction == windCurrentDirection.NW)
                    return EventManager.getImg("ComSeasonGod.property_current_nw_population.png");
                else if (direction == windCurrentDirection.NE)
                    return EventManager.getImg("ComSeasonGod.property_current_ne_population.png");
                else if (direction == windCurrentDirection.SE)
                    return EventManager.getImg("ComSeasonGod.property_current_se_population.png");
                else if (direction == windCurrentDirection.SW)
                    return EventManager.getImg("ComSeasonGod.property_current_sw_population.png");
                return EventManager.getImg("ComSeasonGod.property_current_generic_population.png");
            }

            if (direction == windCurrentDirection.NW)
                return EventManager.getImg("ComSeasonGod.property_current_nw.png");
            else if (direction == windCurrentDirection.NE)
                return EventManager.getImg("ComSeasonGod.property_current_ne.png");
            else if (direction == windCurrentDirection.SE)
                return EventManager.getImg("ComSeasonGod.property_current_se.png");
            else if (direction == windCurrentDirection.SW)
                return EventManager.getImg("ComSeasonGod.property_current_sw.png");
            return EventManager.getImg("ComSeasonGod.property_current_generic.png");
        }

        public override string getDesc()
        {

            string result = "";
            if (effect == windCurrentEffect.CRISIS)
                result += "The Painter's handiwork carries chaos in its wake. If a downwind location has less Plague, Unrest or Devastation than this location, up to " + crisisCostPerTurn + "% Fey Presence a turn is spent to increase these modifiers downwind by up to " + crisisPerTurn + "% each.\n\n";
            else if (effect == windCurrentEffect.SHADOW)
                result += "The Painter's brushstrokes blot out light and hope. If a downwind location has less <b>shadow</b> than this location, up to " + shadowCostPerTurn + "% Fey Presence a turn is spent to increase <b>shadow</b> downwind by up to " + (shadowPerTurn * 100) + "% (redued by any Ward present).\n\n";
            else if (effect == windCurrentEffect.POPULATION)
                result += "Trade winds beckon the people to new horizons, hollowing out the land behind them. Up to " + popCostPerTurn + "% Fey Presence a turn is spent to move " + popPerTurn + " population from this settlement to downwind settlements (increasing the destination's food and maximum population if a city, and limited to 40 if a minor settlement).\n\n";
            else
                result += "Enchanted winds stir up ambient magic, priming the canvas for the Painter's schemes.";

            if (empowered)
            {
                if (direction == windCurrentDirection.NW)
                    return result + "Fey Presence increases by " + chargeDelta + "% per turn in this location and any neighbours to the northwest.";
                else if (direction == windCurrentDirection.NE)
                    return result + "Fey Presence increases by " + chargeDelta + "%  per turn in this location and any neighbours to the northeast.";
                else if (direction == windCurrentDirection.SE)
                    return result + "Fey Presence increases by " + chargeDelta + "% per turn in this location and any neighbours to the southeast.";
                else if (direction == windCurrentDirection.SW)
                    return result + "Fey Presence increases by " + chargeDelta + "% per turn in this location and any neighbours to the southwest.";

                return result + "Fey Presence increases by " + chargeDelta + "% per turn in this location and all its neighbours.";

            }

            if (direction == windCurrentDirection.NW)
                return result + "While the Painter of Winds is active, this modifier increases Fey Presence by " + chargeDelta + "% in this location and any neighbours to the northwest.";
            else if (direction == windCurrentDirection.NE)
                return result + "While the Painter of Winds is active, this modifier increases Fey Presence by " + chargeDelta + "% in this location and any neighbours to the northeast.";
            else if (direction == windCurrentDirection.SE)
                return result + "While the Painter of Winds is active, this modifier increases Fey Presence by " + chargeDelta + "% in this location and any neighbours to the southeast.";
            else if (direction == windCurrentDirection.SW)
                return result + "While the Painter of Winds is active, this modifier increases Fey Presence by " + chargeDelta + "% in this location and any neighbours to the southwest.";

            return result + "While the Painter of Winds is active, this modifier increases Fey Presence by " + chargeDelta + "% in this location and all its neighbours.";
        }

    }
}
