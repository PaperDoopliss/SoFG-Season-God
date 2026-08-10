using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_AshCloud : P_Season
    {

        public static float shadowPerAsh = 0.01f;

        public P_Season_AshCloud(Map map) : base(map) { }

        public override string getName()
        {
            return "Ash Cloud";
        }

        public override string getDesc()
        {
            return "Converts Ashen Earth in a location and its neighbours into an equal amount of <b>shadow</b>, to a maximum of 100%";
        }

        public override string getFlavour()
        {
            return "The ash blanketing the land is kicked up into the sky, blotting out the sun and smothering hope.";
        }

        public override string getRestrictionText()
        {
            return "Must target a location which has Ashen Earth, or whose neighbours have Ashen Earth";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_ash_cloud.png");
        }

        public override bool validTarget(Location loc)
        {
            foreach (Property pr in loc.properties)
            {
                if (pr is Pr_Season_AshenEarth)
                    return true;
            }
            foreach (Location l in loc.getNeighbours())
            {
                foreach (Property pr in l.properties)
                {
                    if (pr is Pr_Season_AshenEarth)
                        return true;
                }
            }
            return false;
        }

        public override int getCost()
        {
            return 3;
        }

        public void convertAshIntoShadow(Location loc)
        {
            for (int i = 0; i < loc.properties.Count; i++)
            {
                if (loc.getShadow() >= 1)
                    return;

                if (loc.properties[i] is Pr_Season_AshenEarth ash)
                {
                    double shadowToAdd = Math.Min(ash.charge * shadowPerAsh, 1 - loc.getShadow());
                    ash.charge -= shadowToAdd / shadowPerAsh;

                    if (loc.settlement != null)
                        loc.settlement.shadow += shadowToAdd;
                    else
                    {
                        foreach (Hex hex in loc.territory)
                        {
                            hex.purity -= (float)shadowToAdd;
                            if (hex.purity < 0)
                                hex.purity = 0;
                        }

                    }


                    if (ash.charge <= 0)
                    {
                        loc.properties.RemoveAt(i);
                        i--;
                    }

                    
                }
            }
        }

        public override void cast(Location loc)
        {
            base.cast(loc);

            convertAshIntoShadow(loc);

            foreach (Location l in loc.getNeighbours())
            {
                convertAshIntoShadow(l);
            }

        }


    }
}