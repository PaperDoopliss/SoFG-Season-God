using Assets.Code;
using CommunityLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_GaleForce : P_Season
    {

        public static double devastationToSpread = 100;
        public static double shadowToSpread = 0.4;
        public static int populationToMove = 40;
        public static double presenceToAdd = 50;
        public static int locationsToHit = 5;

        public P_Season_GaleForce(Map map) : base(map) { }

        public override string getName()
        {
            return "Gale Force";
        }

        public override string getDesc()
        {
            return "A target Wind Current inflicts a bolstered effect on populated downwind locations. A Tumultuous Current inflicts a flat " + devastationToSpread + "% Devastation, a Smothering Current spreads up to " + (shadowToSpread * 100) + "% shadow (reduced by Ward and limited by the source location's shadow), a Beckoning Current moves up to " + populationToMove + " population, and an unmodified Wind Current adds " + presenceToAdd + "% Fey Presence. This power will also trigger on any downwind Wind Currents until at least " + locationsToHit + " locations have been impacted.";
        }

        public override string getFlavour()
        {
            return "The subtle presence of the wind flares into something unignorable, consuming all it touches.";
        }

        public override string getRestrictionText()
        {
            return "Must target a location with a Wind Current";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_primordial_tempest.png");
        }

        public override bool validTarget(Location loc)
        {
            return true;
        }

        public override int getCost()
        {
            return 4;
        }

        public override void cast(Location location)
        {
            base.cast(location);

            List<Location> targets = new List<Location>();
            targets.Add(location);

            int i = 0;
            while (i < locationsToHit)
            {
                if (targets.Count == 0)
                    break;


                List<Location> newTargets = new List<Location>();
                foreach (Location l in targets)
                {
                    foreach (Property pr in l.properties)
                    {
                        if (pr is Pr_Season_WindCurrent current)
                        {
                            foreach (Location l2 in current.downwind)
                            {
                                if (l2.settlement is SettlementHuman sh2 == false)
                                    continue;

                                if (current.effect == Pr_Season_WindCurrent.windCurrentEffect.SHADOW)
                                {
                                    if (sh2.shadow < l.getShadow())
                                    {
                                        double maxPossibleSpread = shadowToSpread;
                                        foreach (Property pr2 in l2.properties)
                                        {
                                            if (pr2 is Pr_Ward)
                                            {
                                                maxPossibleSpread *= 1 - (pr2.charge / 100);
                                            }
                                        }

                                        maxPossibleSpread = Math.Min(maxPossibleSpread, l.getShadow() - sh2.shadow);
                                        if (maxPossibleSpread > 0)
                                        {
                                            sh2.shadow += maxPossibleSpread;
                                            i++;
                                        }
                                    }
                                }
                                else if (current.effect == Pr_Season_WindCurrent.windCurrentEffect.POPULATION)
                                {
                                    if (l.settlement is SettlementHuman sh)
                                    {

                                        int popToMove = Math.Min(sh.population, populationToMove);

                                        if (popToMove > 0)
                                        {
                                            i++;

                                            sh.population -= popToMove;
                                            sh2.population += popToMove;

                                            foreach (Property pr2 in location.properties)
                                            {
                                                if (pr is Pr_Season_IndustriousNewcomers)
                                                {
                                                    pr.charge -= popToMove;
                                                    break;
                                                }
                                            }

                                            if (sh2 is Set_MinorHuman == false)
                                            {
                                                bool propertyFoundInTarget = false;
                                                foreach (Property pr2 in l.properties)
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
                                else if (current.effect == Pr_Season_WindCurrent.windCurrentEffect.CRISIS)
                                {
                                    Property.addToProperty("Gale Force", Property.standardProperties.DEVASTATION, devastationToSpread, l2);
                                    i++;
                                }
                                else
                                {
                                    bool foundFeyPresence = false;

                                    foreach (Property pr2 in l2.properties)
                                    {
                                        if (pr2 is Pr_FeyPresence)
                                        {
                                            foundFeyPresence = true;
                                            pr2.charge += presenceToAdd;
                                            break;
                                        }
                                    }
                                    if (!foundFeyPresence)
                                    {
                                        Pr_FeyPresence presence = new Pr_FeyPresence(l2);
                                        presence.charge = presenceToAdd;
                                        l2.properties.Add(presence);
                                    }
                                    i++;
                                }

                                    foreach (Property pr2 in l2.properties)
                                    {
                                        if (pr2 is Pr_Season_WindCurrent)
                                            newTargets.Add(l2);
                                    }

                                if (i >= locationsToHit)
                                    break;
                            }
                        }

                        if (i >= locationsToHit)
                            break;
                    }
                }


                newTargets = targets;

            }


        }


    }
}
