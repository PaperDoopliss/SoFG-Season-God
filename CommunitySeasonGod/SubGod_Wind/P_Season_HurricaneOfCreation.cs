using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_HurricaneOfCreation : P_Season
    {
        public P_Season_HurricaneOfCreation(Map map) : base(map) { }

        public override string getName()
        {
            return "Hurricane of Creation";
        }

        public override string getDesc()
        {
            return "Alters a Wind Current so that it blows toward all neighbours that don't have a Wind Current blowing toward it.";
        }

        public override string getFlavour()
        {
            return "The winds whirl into an eye of a spiritual storm. As long as the Painter paints, this place will inescapably define the lands around it.";
        }

        public override string getRestrictionText()
        {
            return "Must target a location with a Wind Current that only targets one direction.";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_hurricane_of_creation.png");
        }

        public override bool validTarget(Location loc)
        {
            foreach (Property pr in loc.properties)
            {
                if (pr is Pr_Season_WindCurrent current)
                {
                    if (current.direction != Pr_Season_WindCurrent.windCurrentDirection.All)
                        return true;
                }
            }


            return false;
        }

        public override int getCost()
        {
            return 3;
        }

        public override void cast(Location location)
        {
            base.cast(location);

            foreach (Property pr in location.properties)
            {
                if (pr is Pr_Season_WindCurrent current)
                {
                    current.direction = Pr_Season_WindCurrent.windCurrentDirection.All;
                    current.updateDownwindLocations();
                }
            }
        }


    }
}
