using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_HeatWave : P_Season
    {

        public static float temperatureToAdd = 0.1f;
        public static double duration = 5;
        public static double presenceCutoff = 150;

        public P_Season_HeatWave(Map map) : base(map) { }

        public override string getName()
        {
            return "Heat Wave";
        }

        public override string getDesc()
        {
            return "Increases temperature in a location and its neighbours by " + temperatureToAdd * 100 + "%, or twice that if it has at least " + presenceCutoff + "% Fey Presence, for " + duration + " turns";
        }

        public override string getFlavour()
        {
            return "The Patriarch's radiance was never a subtle thing. Now, as it reaches its full power, the earth itself crumbles under the oppressive heat.";
        }

        public override string getRestrictionText()
        {
            return "Can target any location";
        }


        public override bool validTarget(Unit unit)
        {
            return false;
        }

        public override bool validTarget(Location loc)
        {
            return true;
        }

        public override int getCost()
        {
            return /*4*/0;
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_heat_wave.png");
        }

        public override void cast(Location loc)
        {
            base.cast(loc);

            bool isDoubled = false;
            foreach (Property pr in loc.properties)
            {
                if (pr is Pr_FeyPresence)
                {
                    if (pr.charge >= presenceCutoff)
                    {
                        isDoubled = true;
                    }
                    break;
                }
            }


            Pr_Season_HeatWave heatWave = null;
            if (isDoubled)
                heatWave = new Pr_Season_HeatWave(loc,temperatureToAdd * 2f);
            else
                heatWave = new Pr_Season_HeatWave(loc, temperatureToAdd);

            heatWave.charge = duration;
            loc.properties.Add(heatWave);

            foreach (Hex hex in loc.territory)
            {
                hex.transientTempDelta += temperatureToAdd;
                if (isDoubled)
                    hex.transientTempDelta += temperatureToAdd;
            }



            foreach (Location l in loc.getNeighbours())
            {

                bool isDoubled2 = false;
                foreach (Property pr in l.properties)
                {
                    if (pr is Pr_FeyPresence)
                    {
                        if (pr.charge >= presenceCutoff)
                        {
                            isDoubled2 = true;
                        }
                        break;
                    }
                }

                foreach (Hex hex2 in l.territory)
                {
                    hex2.transientTempDelta += temperatureToAdd;
                    if (isDoubled2)
                        hex2.transientTempDelta += temperatureToAdd;
                }
                Pr_Season_HeatWave heatWave2 = null;

                if (isDoubled2)
                    heatWave2 = new Pr_Season_HeatWave(l, temperatureToAdd * 2f);
                else
                    heatWave2 = new Pr_Season_HeatWave(l, temperatureToAdd);

                heatWave2.charge = duration;
                l.properties.Add(heatWave2);
            }


        }


    }
}