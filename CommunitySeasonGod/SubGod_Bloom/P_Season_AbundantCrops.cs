using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_AbundantCrops : P_Season_LimitedCharges
    {

        public P_Season_AbundantCrops(Map map) : base(map)
        {
            Charges = 1;
            _maxCharges = 1;
        }

        public override string getName()
        {
            return "Abundant Crops (" + Charges + ")";
        }

        public override string getDesc()
        {
            return "Create Fey Crops at a location, allowing the Niece of Blooming Fields to target nearby locations with her other powers. Fey Crops grant their location " + Pr_Season_FeyCrops.prosperityIncrease * 100 + "% <b>prosperity</b>, " + Pr_Season_FeyCrops.foodIncrease + " <b>food</b>, and " + Pr_Season_FeyCrops.presencePerTurn + "% Fey Presence per turn. This power can only be used once, but ignores the normal cost increases from using Fey Crops.";
        }

        public override string getFlavour()
        {
            return "The Niece's exuberance cannot be contained, and her gifts spread across the world.";
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
            return 0;
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
            SpendCharge();
        }



    }
}
