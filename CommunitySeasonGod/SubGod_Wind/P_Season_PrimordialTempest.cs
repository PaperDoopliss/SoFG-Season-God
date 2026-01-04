using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_PrimordialTempest : P_Season
    {
        public P_Season_PrimordialTempest(Map map) : base(map) { }

        public override string getName()
        {
            return "Primordial Tempest";
        }

        public override string getDesc()
        {
            return "Create a random Wind Current with a random effect in every location within two steps that does not already have a Wind Current.";
        }

        public override string getFlavour()
        {
            return "The Painter gives voice to her every whim. The souls in the world are stirred into chaos, swept up into patterns made without concern for the ebb and flow of human life.";
        }

        public override string getRestrictionText()
        {
            return "";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_primordial_tempest.png");
        }

        public override bool validTarget(Location loc)
        {
            return true;
        }

        public override int getCost()
        {
            return 7;
        }

        public override void cast(Location location)
        {
            base.cast(location);

            List<Location> targets = new List<Location>();
            targets.Add(location);

            List<Location> neighbours = location.getNeighbours();
            targets.AddRange(neighbours);

            foreach (Location l in neighbours)
            {
                foreach (Location l2 in l.getNeighbours())
                {
                    if (targets.Contains(l2) == false)
                        targets.Add(l2);
                }
            }

            foreach (Location l in targets)
            {
                bool foundWindCurrent = false;
                foreach (Property pr in l.properties)
                {
                    if (pr is Pr_Season_WindCurrent)
                    {
                        foundWindCurrent = true;
                        break;
                    }
                }

                if (!foundWindCurrent)
                {
                    Pr_Season_WindCurrent newWindCurrent = new Pr_Season_WindCurrent(l, Pr_Season_WindCurrent.windCurrentDirection.All);
                    int effect = Eleven.random.Next(3);
                    if (effect == 0)
                        newWindCurrent.effect = Pr_Season_WindCurrent.windCurrentEffect.CRISIS;
                    else if (effect == 1)
                        newWindCurrent.effect = Pr_Season_WindCurrent.windCurrentEffect.SHADOW;
                    else
                        newWindCurrent.effect = Pr_Season_WindCurrent.windCurrentEffect.POPULATION;

                    int direction = Eleven.random.Next(5);
                    if (direction == 0)
                        newWindCurrent.direction = Pr_Season_WindCurrent.windCurrentDirection.NE;
                    else if (direction == 1)
                        newWindCurrent.direction = Pr_Season_WindCurrent.windCurrentDirection.NW;
                    else if (direction == 2)
                        newWindCurrent.direction = Pr_Season_WindCurrent.windCurrentDirection.SW;
                    else if (direction == 3)
                        newWindCurrent.direction = Pr_Season_WindCurrent.windCurrentDirection.NE;
                    else
                        newWindCurrent.direction = Pr_Season_WindCurrent.windCurrentDirection.All;

                    newWindCurrent.charge = 100;
                    l.properties.Add(newWindCurrent);
                    newWindCurrent.updateDownwindLocations();
                }
            }

        }


    }
}
