using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_FeyCrops : P_Season
    {
        public int cost = 0;
        public static int costIncrease = 1;

        public P_Season_FeyCrops(Map map) : base(map)
        {
        }

        public override string getName()
        {
            return "Fey Crops";
        }

        public override string getDesc()
        {
            return "Create Fey Crops at a location, allowing the Niece of Blooming Fields to target nearby locations with her other powers. Fey Crops grant their location " + Pr_Season_FeyCrops.prosperityIncrease * 100 + "% <b>prosperity</b>, " + Pr_Season_FeyCrops.foodIncrease + " <b>food</b>, and " + Pr_Season_FeyCrops.presencePerTurn + "% Fey Presence per turn. This power's cost increases by " + costIncrease + " every time it is used.";
        }

        public override string getFlavour()
        {
            return "If life can be seeded in the farmlands, the ebb and flow of humanity will carry it across the land. For this to happen, the life must be useful.";
        }

        public override string getRestrictionText()
        {
            return "Must target a Farming Community with an unaware ruler and no existing Fey Crops";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_fey_crops.png");
        }

        public override bool validTarget(Location loc)
        {

            if (loc.settlement is SettlementHuman sh)
            {
                if (sh.ruler == null || sh.ruler.awareness < 1)
                {
                    foreach (Property pr in loc.properties)
                    {
                        if (pr is Pr_Season_FeyCrops)
                            return false;
                    }


                    foreach (Subsettlement sub in sh.subs)
                    {
                        if (sub is Sub_Farms)
                            return true;
                    }
                }
            }

            return false;
        }

        public override int getCost()
        {
            return cost;
        }

        public override void cast(Location loc)
        {
            base.cast(loc);

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
            cost += costIncrease;
        }



    }
}
