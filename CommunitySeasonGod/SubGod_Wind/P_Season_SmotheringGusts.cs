using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_SmotheringGusts : P_Season
    {
        public P_Season_SmotheringGusts(Map map) : base(map) { }

        public override string getName()
        {
            return "Smothering Gusts";
        }

        public override string getDesc()
        {
            return "Turns a Wind Current into a Smothering Current. In addition to normal effects, if a downwind location has less <b>shadow</b> than this location, up to " + Pr_Season_WindCurrent.shadowCostPerTurn + "% Fey Presence a turn is spent to increase <b>shadow</b> downwind by up to " + (Pr_Season_WindCurrent.shadowPerTurn * 100) + "% (redued by any Ward present).";
        }

        public override string getFlavour()
        {
            return "The darkness spreads with the Painter's touch, the shadow on the horizon carrying a weight of inevitability.";
        }

        public override string getRestrictionText()
        {
            return "Must target a location with a Non-Smothering Wind Current";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_smothering_gusts.png");
        }

        public override bool validTarget(Location loc)
        {
            foreach (Property pr in loc.properties)
            {
                if (pr is Pr_Season_WindCurrent current)
                {
                    if (current.effect != Pr_Season_WindCurrent.windCurrentEffect.SHADOW)
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
                    current.effect = Pr_Season_WindCurrent.windCurrentEffect.SHADOW;
                    return;
                }
            }
        }


    }
}
