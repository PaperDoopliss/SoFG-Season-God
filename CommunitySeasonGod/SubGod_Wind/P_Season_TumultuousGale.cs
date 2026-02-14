using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_TumultuousGale : P_Season
    {
        public P_Season_TumultuousGale(Map map) : base(map) { }

        public override string getName()
        {
            return "Tumultuous Gale";
        }

        public override string getDesc()
        {
            return "Turns a Wind Current into a Tumultuous Current. In addition to normal effects, if its location has Plague, Devastation or Unrest, it increases those modifiers in downwind locations by " + Pr_Season_WindCurrent.crisisPerTurn + "% if lower at the cost of " + Pr_Season_WindCurrent.crisisCostPerTurn + "% Fey Presence per turn.";
        }

        public override string getFlavour()
        {
            return "The Painter's brushstrokes smear chaos across the world, breaking down order and ruining lives.";
        }

        public override string getRestrictionText()
        {
            return "Must target a location with a non-Tumultuous Wind Current";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_tumultuous_gale.png");
        }

        public override bool validTarget(Location loc)
        {
            foreach (Property pr in loc.properties)
            {
                if (pr is Pr_Season_WindCurrent current)
                {
                    if (current.effect != Pr_Season_WindCurrent.windCurrentEffect.CRISIS)
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
                    current.effect = Pr_Season_WindCurrent.windCurrentEffect.CRISIS;
                    return;
                }
            }
        }


    }
}
