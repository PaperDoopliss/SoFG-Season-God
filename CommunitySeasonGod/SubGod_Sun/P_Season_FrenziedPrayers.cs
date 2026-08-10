using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_FrenziedPrayers : P_Season_LimitedCharges
    {

        public static double unrestToAddInNeighbours = 50;
        public static double presenceToAdd = 75;
        public static double ashenEarthToAdd = 25;

        public P_Season_FrenziedPrayers(Map map) : base(map) { }

        public override string getName()
        {
            return "Frenzied Prayers (" + Charges + ")";
        }

        public override string getDesc()
        {
            return "Grants a location " + presenceToAdd + "% Fey Presence and " + ashenEarthToAdd + " Ashen Earth, and increases Unrest in neighbouring locations by " + unrestToAddInNeighbours + "%";
        }

        public override string getFlavour()
        {
            return "The Patriarch's cult is very difficult to ignore.";
        }

        public override string getRestrictionText()
        {
            return "This power can only be used once";
        }

        public override bool validTarget(Location loc)
        {
            if (loc.isOcean == false)
                return true;
            return false;
        }

        public override int getCost()
        {
            return 0;
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_frenzied_prayers.png");
        }

        public override void cast(Location loc)
        {
            base.cast(loc);

            Pr_FeyPresence presence = null;
            Pr_Season_AshenEarth ash = null;
            foreach (Property pr in loc.properties)
            {
                if (pr is Pr_FeyPresence found)
                    presence = found;
                else if (pr is Pr_Season_AshenEarth foundAsh)
                    ash = foundAsh;
            }

            if (presence == null)
            {
                presence = new Pr_FeyPresence(loc);
                presence.charge = presenceToAdd;
                loc.properties.Add(presence);
            }
            else
                presence.charge += presenceToAdd;

            if (ash == null)
            {
                ash = new Pr_Season_AshenEarth(loc);
                ash.charge = ashenEarthToAdd;
                loc.properties.Add(ash);
            }
            else
                ash.charge += ashenEarthToAdd;

            foreach (Location l in loc.getNeighbours())
            {
                Property.addToPropertySingleShot("Frenzied Prayers", Property.standardProperties.UNREST, unrestToAddInNeighbours, l);
            }

            SpendCharge();
        }


    }
}