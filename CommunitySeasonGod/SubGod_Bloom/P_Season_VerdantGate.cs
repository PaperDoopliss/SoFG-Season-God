using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_VerdantGate : P_Season_LimitedCharges
    {
        public static double presenceCost = 300;
        public static int range = 4;

        public P_Season_VerdantGate(Map map) : base(map) { Charges = 1; _maxCharges = 1; }

        public override string getName()
        {
            return "Verdant Gate (" + Charges + ")";
        }

        public override string getDesc()
        {
            return "Spends " + presenceCost + "% Fey Presence to create a Verdant Gate at a location. It will max out the location's <b>shadow</b>, add Fey Crops and Blooming Fields, increase Madness by " + Sub_Season_VerdantGate.madnessPerTurn + "% per turn and Dreaming Kudzu by " + Sub_Season_VerdantGate.kudzuPerTurn + " and another " + Sub_Season_VerdantGate.kudzuInNeighbours + "% in neighbouring land locations, and summons a distracting Vine Terror every " + Sub_Season_VerdantGate.vineTerrorSpawnCooldown + " turns. This power can only be used once.";
        }

        public override string getFlavour()
        {
            return "The Niece’s gardens scale beyond mortal comprehension, a towering mass of foliage that can boggle the mind. When her Season reaches its peak, she can tear a hole in reality to bring her gardens forth into the mortal realm.";
        }

        public override string getRestrictionText()
        {
            return "Must target a land settlement with at least " + presenceCost + " Fey Presence within " + range + " steps of Fey Crops that does not have Purged Kudzu present";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_verdant_gate.png");
        }


        public override bool validTarget(Location loc)
        {
            if (loc.isOcean || loc.settlement == null)
                return false;

            double presence = 0;
            foreach (Property pr in loc.properties)
            {
                if (pr is Pr_FeyPresence)
                {
                    presence += pr.charge;
                }
            }

            if (presence >= presenceCost)
            {
                if (map.overmind.god is God_Season season && season.ActiveSubGod is SubGod_Bloom bloom)
                {
                    foreach (Property pr2 in bloom.crops)
                    {

                        if (map.getStepDist(pr2.location, loc) <= range)
                        {
                            return true;
                        }
                    }

                    return false;
                }
                return true;
            }

            

            return false;
        }

        public override int getCost()
        {
            return 9;
        }

        public override void cast(Location loc)
        {
            base.cast(loc);

            double presenceSpent = 0;

            for (int i = 0; i < loc.properties.Count; i++)
            {
                if (loc.properties[i] is Pr_FeyPresence)
                {
                    if (loc.properties[i].charge >= presenceCost - presenceSpent)
                    {
                        loc.properties[i].charge -= presenceCost - presenceSpent;
                        presenceSpent = presenceCost;
                    }
                    else
                    {
                        presenceSpent += loc.properties[i].charge;
                        loc.properties.RemoveAt(i);
                        i--;
                    }
                }
            }

            loc.settlement?.subs.Add(new Sub_Season_VerdantGate(loc.settlement));
            

            loc.properties.Add(new Pr_Season_BloomingFields(loc));

            bool foundCrops = false;
            foreach (Property pr in loc.properties)
            {
                if (pr is Pr_Season_FeyCrops)
                {
                    foundCrops = true;
                    break;
                }
            }

            if (!foundCrops)
            {
                Pr_Season_FeyCrops crops = new Pr_Season_FeyCrops(loc);
                loc.properties.Add(crops);
                if (map.overmind.god is God_Season season)
                {

                    foreach (SubGod subGod in season.SubGods)
                    {
                        if (subGod is SubGod_Bloom bloom)
                            bloom.crops.Add(crops);
                    }

                }
            }
            SpendCharge();
        }


    }
}
