using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static CommunitySeasonGod.Pr_Season_WindCurrent;

namespace CommunitySeasonGod
{
    public class P_Season_WindCurrent : P_Season
    {

        public static int cost = 1;

        public P_Season_WindCurrent(Map map) : base(map) { }

        public override string getName()
        {
            return "Wind Current";
        }

        public override string getDesc()
        {
            return "Places a Wind Current in a location that has Fey Presence, or that is next to an Ocean location. The Wind Current will generate " + Pr_Season_WindCurrent.chargeDelta + "% Fey Presence per turn in its own location and any locations to the northwest, northeast, southeast or southwest, and can be upgraded to affect the world more directly.\n\nIf a location with a Wind Current is targeted it loses any additional effects, its direction can be changed, and the cost is refunded.";
        }

        public override string getFlavour()
        {
            return "A brushstroke in the ether leaves wrinkles between worlds. Travelers shiver as they cross it without knowing why.";
        }

        public override string getRestrictionText()
        {
            return "Must target a location next to an ocean location, or that has Fey Presence. Cannot blow in the same direction as a Wind Current already blowing into the target location.";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.property_current_sw.png");
        }

        public override bool validTarget(Location loc)
        {
            foreach (Property pr in loc.properties)
            {
                if (pr is Pr_Season_WindCurrent)
                    return true;
                else if (pr is Pr_FeyPresence)
                    return true;
            }

            foreach (Location l in loc.getNeighbours())
            {
                if (l.isOcean)
                    return true;
            }

            return false;
        }

        public override int getCost()
        {
            return cost;
        }

        public override void cast(Location location)
        {
            base.cast(location);

            bool SWUpwind = false;
            bool SEUpwind = false;
            bool NEUpwind = false;
            bool NWUpwind = false;

            foreach (Location l in location.getNeighbours())
            {

                foreach (Property pr in l.properties)
                {
                    if (pr is Pr_Season_WindCurrent current)
                    {

                        //Horizontals
                        if (l.hex.y == location.hex.y && l.hex.x >= location.hex.x)
                        {
                            if (current.direction == windCurrentDirection.All || current.direction == windCurrentDirection.SE || current.direction == windCurrentDirection.NE)
                            {
                                SWUpwind = true;
                                NWUpwind = true;
                            }
                        }
                        else if (l.hex.y == location.hex.y && l.hex.x <= location.hex.x)
                        {
                            if (current.direction == windCurrentDirection.All || current.direction == windCurrentDirection.SW || current.direction == windCurrentDirection.NW)
                            {
                                SEUpwind = true;
                                NEUpwind = true;
                            }
                        }
                        else if (l.hex.y <= location.hex.y && l.hex.x == location.hex.x && l.hex.y % 2 == location.hex.y % 2)
                        {
                            if (current.direction == windCurrentDirection.All || current.direction == windCurrentDirection.SE || current.direction == windCurrentDirection.SW)
                            {
                                NWUpwind = true;
                                NEUpwind = true;
                            }
                        }
                        else if (l.hex.y >= location.hex.y && l.hex.x == location.hex.x && l.hex.y % 2 == location.hex.y % 2)
                        {
                            if (current.direction == windCurrentDirection.All || current.direction == windCurrentDirection.NE || current.direction == windCurrentDirection.NW)
                            {
                                SWUpwind = true;
                                SEUpwind = true;
                            }
                        }

                        //Diagonals (same X, but it's offset)
                        else if (l.hex.y <= location.hex.y && l.hex.x == location.hex.x && l.hex.y % 2 != location.hex.y % 2)
                        {
                            if (current.direction == windCurrentDirection.All || current.direction == windCurrentDirection.SW)
                            {
                                NEUpwind = true;
                            }
                        }
                        else if (l.hex.y >= location.hex.y && l.hex.x == location.hex.x && l.hex.y % 2 != location.hex.y % 2)
                        {
                            if (current.direction == windCurrentDirection.All || current.direction == windCurrentDirection.NE)
                            {
                                SWUpwind = true;
                            }
                        }

                        //Diagonals
                        else if (l.hex.y < location.hex.y && l.hex.x > location.hex.x)
                        {
                            if (current.direction == windCurrentDirection.All || current.direction == windCurrentDirection.SE)
                                NWUpwind = true;
                        }
                        else if (l.hex.y < location.hex.y && l.hex.x < location.hex.x)
                        {
                            if (current.direction == windCurrentDirection.All || current.direction == windCurrentDirection.SW)
                                NEUpwind = true;
                        }
                        else if (l.hex.y > location.hex.y && l.hex.x < location.hex.x)
                        {
                            if (current.direction == windCurrentDirection.All || current.direction == windCurrentDirection.NW)
                                SEUpwind = true;
                        }
                        else if (l.hex.y > location.hex.y && l.hex.x >= location.hex.x)
                        {
                            if (current.direction == windCurrentDirection.All || current.direction == windCurrentDirection.NE)
                                SWUpwind = true;
                        }

                        break;
                    }
                }
            }

            List<string> options = new List<string>();
            if (SWUpwind == false)
                options.Add("Southwest");
            if (SEUpwind == false)
                options.Add("Southeast");
            if (NEUpwind == false)
                options.Add("Northeast");
            if (NWUpwind == false)
                options.Add("Northwest");

            map.world.ui.addBlocker(map.world.prefabStore.getScrollSetText(options, invertOrder: false, new Sel2_Season_WindCurrent(map,location), "Wind Current", "Choose a direction for the Wind Current").gameObject);
        }

        /*public override void cast(Location loc)
        {
            base.cast(loc);
            if (loc.person() == null)
            {
                return;
            }

            Sel2_ForIdleHands receiver = new Sel2_ForIdleHands(map, loc.person());
            List<string> list = new List<string>();
            string[] names = Tags.names;
            foreach (string text in names)
            {
                bool flag = false;
                foreach (int like in loc.person().likes)
                {
                    if (Tags.getName(like) == text)
                    {
                        flag = true;
                    }
                }

                if (!flag)
                {
                    list.Add(text);
                }
            }

            string[] namesEnemies = Tags.namesEnemies;
            foreach (string text2 in namesEnemies)
            {
                bool flag2 = false;
                foreach (int like2 in loc.person().likes)
                {
                    if (Tags.getName(like2) == text2)
                    {
                        flag2 = true;
                    }
                }

                if (!flag2)
                {
                    list.Add(text2);
                }
            }

            map.world.ui.addBlocker(loc.map.world.prefabStore.getScrollSetText(list, invertOrder: false, receiver, "For Idle Hands", "Choose a tag to make " + loc.person().getName() + " like").gameObject);
        }*/



    }
}
