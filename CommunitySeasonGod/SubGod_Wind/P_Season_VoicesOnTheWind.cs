using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_VoicesOnTheWind : P_Season
    {
        public P_Season_VoicesOnTheWind(Map map) : base(map) { }

        public override string getName()
        {
            return "Voices on the Wind";
        }

        public override string getDesc()
        {
            return "Turns a Wind Current into a Mesmerizing Current. In addition to normal effects, up to " + Pr_Season_WindCurrent.popCostPerTurn + "% Fey Presence a turn is spent to move " + Pr_Season_WindCurrent.popPerTurn + " population from this settlement to downwind settlements (increasing the destination's food and maximum population if a city, and limited to 40 if a minor settlement).";
        }

        public override string getFlavour()
        {
            return "The people find themselves drifting along with the current in search of a better life, leaving empty homes and untended fields behind them.";
        }

        public override string getRestrictionText()
        {
            return "Must target a location with a Non-Mesmerizing Wind Current";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_voices_on_the_wind.png");
        }

        public override bool validTarget(Location loc)
        {
            foreach (Property pr in loc.properties)
            {
                if (pr is Pr_Season_WindCurrent current)
                {
                    if (current.effect != Pr_Season_WindCurrent.windCurrentEffect.POPULATION)
                        return true;
                }
            }


            return false;
        }

        public override int getCost()
        {
            return 1;
        }

        public override void cast(Location location)
        {
            base.cast(location);

            foreach (Property pr in location.properties)
            {
                if (pr is Pr_Season_WindCurrent current)
                {
                    current.effect = Pr_Season_WindCurrent.windCurrentEffect.POPULATION;
                    return;
                }
            }
        }


    }
}
