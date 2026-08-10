using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_SolarAbsorption : P_Season
    {
        public static double conversionRate = 50;

        public P_Season_SolarAbsorption(Map map) : base(map) { }

        public override string getName()
        {
            return "Solar Absorption";
        }

        public override string getDesc()
        {
            return "Increases a Geomantic Locus' charge by " + conversionRate + "% of the location's temperature";
        }

        public override string getFlavour()
        {
            return "The heat seeps into the earth, infuses the ley lines, and lies in wait.";
        }

        public override string getRestrictionText()
        {
            return "Must target a location with a geomantic locus";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_solar_absorption.png");
        }

        public override bool validTarget(Unit unit)
        {
            return false;
        }

        public override bool validTarget(Location loc)
        {
            foreach (Property prop in loc.properties)
                if (prop is Pr_GeomanticLocus)
                {
                    return true;
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
            foreach (Property prop in loc.properties)
                if (prop is Pr_GeomanticLocus)
                {
                    prop.charge += (float)loc.hex.getTemperature() * conversionRate;
                }
        }


    }
}