using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CommunitySeasonGod
{

    //DLC: UPDATE SO IT CAN'T BE TARGETED UNDERGROUND

    public class P_Season_Wildfire : P_Season
    {

        public static double presenceCost = 150;
        public static double presenceCutoff = 250;
        public static double startingCharge = 200;

        public P_Season_Wildfire(Map map) : base(map) { }


        public override string getName()
        {
            return "Wildfire";
        }

        public override string getDesc()
        {
            return "Spends " + presenceCost + "% Fey Presence to create a Wildfire modifier at " + startingCharge + " charge which increases by " + Pr_Season_Wildfire.increasePerTurn + "%, to a maximum of 300%. If the Wildfire has " + Pr_Season_Wildfire.devastationCutoff + "% charge or more, Devastation will increase by the same amount, to a maximum of the Wildfire's charge. If the location has Fey Presence of at least " + Pr_Season_Wildfire.presenceCutoff + ", the Wildfire will switch to burning Fey Presence, doubling the speed of its growth and converting " + Pr_Season_Wildfire.presenceBurnedPerTurn + "% Fey Presence a turn into Ashen Earth until the Fey Presence is entirely gone. Once in this Wildfire's lifetime, if its charge is at least " + Pr_Season_Wildfire.spreadCutoff + ", it can spread to a neighbouring location that has a Plains, Grass, Jungle, Highland, or Forest on its hex. Heroes within 3 links will be motivated to fight the fire if its charge is at least 100, and heroes within 7 links will be motivated to fight the fire if its charge is at least 200. After " + Pr_Season_Wildfire.startingCooldown + " combined turns where the Wildfire's charge is below " + Pr_Season_Wildfire.burnoutCutoff + "% or at 300%, the Wildfire will begin dying down, decreasing by " + Pr_Season_Wildfire.burnoutPerTurn + "% instead of increasing.";
        }

        public override string getFlavour()
        {
            return "The rifts between worlds ignite, ravaging the earth and turning the consumed life into a mystically-potent ash.";
        }

        public override string getRestrictionText()
        {
            return "Must target a location whose hex has either Forest or Plains, and has at least " + presenceCutoff + " Fey Presence.";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_wildfire.png");
        }

        public override bool validTarget(Location loc)
        {

            foreach (Property prop in loc.properties)
            {
                if (prop is Pr_FeyPresence)
                {
                    if (prop.charge >= presenceCutoff)
                    {
                        if (loc.hex.terrain == Hex.terrainType.PLAINS || loc.hex.isForest)
                            return true;
                    }
                }
            }
            return false;
        }

        public override int getCost()
        {
            return 2;
        }

        public override void cast(Location loc)
        {
            base.cast(loc);

            Pr_Season_Wildfire wildfire = new Pr_Season_Wildfire(loc);
            wildfire.charge = startingCharge;
            loc.properties.Add(wildfire);



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

        }
    }
}