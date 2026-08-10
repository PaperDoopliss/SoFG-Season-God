using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Pr_Season_HeatWave : Property
    {

        public float addedTemp = 0f;

        public Pr_Season_HeatWave(Location loc, float addedTemp)
            : base(loc)
        {
            this.addedTemp = addedTemp;
        }

        public override string getName()
        {
            return "Heat Wave";
        }

        public override string getDesc()
        {
            return "This location's temperature has increased by " + addedTemp + "%, and will revert once this modifier disappears.";
        }

        public override Sprite getSprite(World world)
        {
            return EventManager.getImg("ComSeasonGod.power_heat_wave.png");
        }

        public override bool deleteOnZero()
        {
            return false;
        }

        public override void turnTick()
        {
            influences.Add(new ReasonMsg("Fading Heat", -1));
            if (charge <= 0)
            {
                foreach (Hex hex in location.territory)
                {
                    hex.transientTempDelta -= (float)(addedTemp - (map.param.map_tempTemperatureReversion * P_Season_HeatWave.duration));
                }

                location.properties.Remove(this);
            }
        }
    }
}
