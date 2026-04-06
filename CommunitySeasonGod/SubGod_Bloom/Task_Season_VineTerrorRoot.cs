using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;

namespace CommunitySeasonGod
{
    public class Task_Season_VineTerrorRoot : Assets.Code.Task
    {
        public Location target = null;
        public Unit unit = null;
        public static int complexity = 10;
        public int progress = 0;
        public static float shadowToAdd = 0.15f;
        public static double presenceToAdd = 50;

        public Task_Season_VineTerrorRoot(Location target, Unit unit) : base()
        {
            this.target = target;
            this.unit = unit;
        }

        public override string getShort()
        {
            if (unit.location != target)
                return "Going to Root in " + target.getName();
            return "Spreading Roots (" + progress + "/" + complexity + ")";
        }
        public override string getLong()
        {
            if (unit.location != target)
                return "This unit is moving to root and spread shadow and Fey Presence at " + target.getName();
            return "This unit is spreading roots in " + target.getName() + " to spread shadow and Fey Presence";
        }

        public override void turnTick(Unit unit)
        {
            if (target.settlement is SettlementHuman sh == false)
            {
                unit.task = null;
                return;
            }

            if (unit.location == target)
            {
                progress++;

                if (progress >= complexity)
                {
                    sh.shadow = Math.Min(1, sh.shadow + shadowToAdd);
                    bool foundFeyCrops = false;
                    bool foundPresence = false;
                    foreach (Property pr in target.properties)
                    {
                        if (pr is Pr_Season_FeyCrops)
                            foundFeyCrops = true;
                        else if (pr is Pr_FeyPresence)
                        {
                            foundPresence = true;
                            pr.influences.Add(new ReasonMsg("Vine Terror", presenceToAdd));
                        }
                    }

                    if (!foundFeyCrops)
                    {
                        Pr_Season_FeyCrops crops = new Pr_Season_FeyCrops(target);
                        if (unit.map.overmind.god is God_Season season)
                        {
                            foreach (SubGod subGod in season.SubGods)
                            {
                                if (subGod is SubGod_Bloom bloom)
                                    bloom.crops.Add(crops);
                            }
                        }
                        target.properties.Add(crops);
                    }

                    if (!foundPresence)
                    {
                        Pr_FeyPresence presence = new Pr_FeyPresence(target);
                        presence.charge = presenceToAdd;
                        target.properties.Add(presence);
                    }

                    unit.die(unit.map, unit.getName() + " burrowed into the ground at " + target.getName());
                }
            }
            else
            {
                if (unit.movesTaken > 0)
                {
                    return;
                }

                while (unit.movesTaken < unit.getMaxMoves())
                {
                    Location[] pathTo = unit.location.map.getPathTo(unit.location, target, unit);
                    if (pathTo == null || pathTo.Length < 2)
                    {
                        unit.task = null;
                        break;
                    }

                    unit.location.map.adjacentMoveTo(unit, pathTo[1]);
                    unit.movesTaken++;
                    if (unit.location == target)
                    {
                        break;
                    }
                }
            }
        }

        public override Location getLocation()
        {
            return target;
        }



    }
}
