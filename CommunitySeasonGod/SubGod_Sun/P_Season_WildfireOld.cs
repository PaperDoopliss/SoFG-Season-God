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

    public class P_Season_WildfireOld : P_Season
    {

        public static double presenceBurnPerTurn = 5;
        public static double devastationPerPresence = 2;
        public static double ashPerPresence = 1;

        public P_Season_WildfireOld(Map map) : base(map) { }


        public override string getName()
        {
            return "Wildfire";
        }

        public override string getDesc()
        {
            return "Creates a Wildfire modifier that burns through up to " + presenceBurnPerTurn + " Fey Presence per turn, transforming each point into " + devastationPerPresence + "% Devastation and " + ashPerPresence + "% Ashen Earth. Ashen Earth can be used to fuel later powers, and the Wildfire can spread to neighbouring land locations with Fey Presence.";
        }

        public override string getFlavour()
        {
            return "The rifts between worlds ignite, ravaging the earth and turning the consumed life into a mystically-potent ash.";
        }

        public override string getRestrictionText()
        {
            return "Must target a land location with Fey Presence that does not currently have a Wildfire.";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_wildfire.png");
        }

        public override bool validTarget(Location loc)
        {
            bool presenceFound = false;
            foreach (Property prop in loc.properties)
            {
                if (prop is Pr_FeyPresence)
                {
                    presenceFound = true;
                }
                else if (prop is Pr_Season_WildfireOld)
                    return false;
            }
            if (presenceFound)
                return true;
            return false;
        }

        public override int getCost()
        {
            return 0/*2*/;
        }

        public override void cast(Location loc)
        {
            base.cast(loc);

            loc.properties.Add(new Pr_Season_WildfireOld(loc));

        }
    }
}