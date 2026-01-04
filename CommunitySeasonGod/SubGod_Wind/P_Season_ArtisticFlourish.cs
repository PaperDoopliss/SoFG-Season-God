using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_ArtisticFlourish : P_Season_LimitedCharges
    {

        public static double presenceToAdd = 35;

        public P_Season_ArtisticFlourish(Map map) : base(map) { }

        public override string getName()
        {
            return "Artistic Flourish (" + Charges + ")";
        }

        public override string getDesc()
        {
            return "Adds " + presenceToAdd + "% Fey Presence to a location and all of its neighbours.";
        }

        public override string getFlavour()
        {
            return "Dust devils whip into motion where the air used to be still as the Painter's prelude begins.";
        }

        public override string getRestrictionText()
        {
            return "This power can only be used once.";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_artistic_flourish.png");
        }

        public override bool validTarget(Location loc)
        {
            return true;
        }

        public override int getCost()
        {
            return 0;
        }

        public override void cast(Location location)
        {
            base.cast(location);

            bool foundPresence = false;
            foreach (Property pr in location.properties)
            {
                if (pr is Pr_FeyPresence)
                {
                    foundPresence = true;
                    pr.charge += presenceToAdd;
                    break;
                }
            }
            if (!foundPresence)
            {
                Pr_FeyPresence presence = new Pr_FeyPresence(location);
                presence.charge = presenceToAdd;
                location.properties.Add(presence);
            }

            foreach (Location l in location.getNeighbours())
            {
                bool foundPresence2 = false;
                foreach (Property pr in l.properties)
                {
                    if (pr is Pr_FeyPresence)
                    {
                        foundPresence2 = true;
                        pr.charge += presenceToAdd;
                        break;
                    }
                }
                if (!foundPresence2)
                {
                    Pr_FeyPresence presence = new Pr_FeyPresence(l);
                    presence.charge = presenceToAdd;
                    l.properties.Add(presence);
                }
            }

            SpendCharge();
        }


    }
}
