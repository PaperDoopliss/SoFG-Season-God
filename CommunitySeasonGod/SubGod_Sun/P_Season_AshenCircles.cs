using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_AshenCircles : P_Season
    {
        public static double maxDevastation = 100;

        public P_Season_AshenCircles(Map map) : base(map)
        {
        }

        public override string getName()
        {
            return "Ashen Circles";
        }

        public override string getDesc()
        {
            return "Replaces up to " + maxDevastation + "% of Devastation in a location with Fey Presence.";
        }

        public override string getFlavour()
        {
            return "Rubble and ruin rearranges itself into strange new shapes, creating new conduits for the Patriarch's light.";
        }

        public override string getRestrictionText()
        {
            return "Must target a location with Devastation";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_ashen_circles.png");
        }

        public override bool validTarget(Location loc)
        {

            foreach (Property pr in loc.properties)
            {
                if (pr is Pr_Devastation)
                {
                    return true;
                }
            }

            return false;
        }

        public override int getCost()
        {
            return 1;
        }

        public override void cast(Location loc)
        {
            base.cast(loc);

            double devastationRemoved = 0;
            for (int i = 0; i < loc.properties.Count; i++)
            {
                if (loc.properties[i] is Pr_Devastation)
                {
                    if (loc.properties[i].charge < maxDevastation - devastationRemoved)
                    {
                        devastationRemoved += loc.properties[i].charge;
                        loc.properties.RemoveAt(i);
                    }
                    else
                    {
                        loc.properties[i].charge -= maxDevastation - devastationRemoved;
                        devastationRemoved = maxDevastation;
                        break;
                    }
                }
            }

            bool foundFeyPresence = false;
            foreach (Property pr in loc.properties)
            {
                if (pr is Pr_FeyPresence)
                {
                    foundFeyPresence = true;
                    pr.charge += devastationRemoved;
                }
            }
            if (!foundFeyPresence)
            {
                Pr_FeyPresence presence = new Pr_FeyPresence(loc);
                presence.charge = devastationRemoved;
                loc.properties.Add(presence);
            }
        }



    }
}
