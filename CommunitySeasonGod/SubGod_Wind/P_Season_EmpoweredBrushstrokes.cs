using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_EmpoweredBrushstrokes : P_Season
    {
        public P_Season_EmpoweredBrushstrokes(Map map) : base(map) { }

        public override string getName()
        {
            return "Empowered Brushstrokes";
        }

        public override string getDesc()
        {
            return "Empowers a Wind Current, causing it and its downwind locations to gain an additional " + (Pr_Season_WindCurrent.chargeDeltaEmpowered - Pr_Season_WindCurrent.chargeDelta) + "% Fey Presence per turn, and to generate that Fey Presence even if the Painter of Winds is not active. This effect can be combined with other changes to Wind Currents.";
        }

        public override string getFlavour()
        {
            return "The Painter's focused attention warps the land, preparing nature and society as if it was a canvas.";
        }

        public override string getRestrictionText()
        {
            return "Must target a location with a Non-Empowered Wind Current";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_empowering_brushstrokes.png");
        }

        public override bool validTarget(Location loc)
        {
            foreach (Property pr in loc.properties)
            {
                if (pr is Pr_Season_WindCurrent current)
                {
                    if (current.empowered == false)
                        return true;
                }
            }


            return false;
        }

        public override int getCost()
        {
            return 4;
        }

        public override void cast(Location location)
        {
            base.cast(location);

            foreach (Property pr in location.properties)
            {
                if (pr is Pr_Season_WindCurrent current)
                {
                    current.empowered = true;
                }
            }
        }


    }
}
