using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;

namespace CommunitySeasonGod
{
    public class Task_Season_RazeKudzu : Assets.Code.Task
    {
        public Pr_Season_DreamingKudzu kudzu;
        public Unit unit;

        public Task_Season_RazeKudzu(Pr_Season_DreamingKudzu kudzu, Unit unit) : base()
        {
            this.kudzu = kudzu;
            this.unit = unit;
        }

        public override string getShort()
        {
            if (unit.location != kudzu.location)
                return "Going to Raze Kudzu";
            return "Razing Kudzu";
        }
        public override string getLong()
        {
            if (unit.location != kudzu.location)
                return "This unit is moving to raze the Dreaming Kudzu at " + kudzu.location.getName();
            return "This unit is razing the Dreaming Kudzu at their location";
        }

        public override void turnTick(Unit unit)
        {
            if (unit.location == kudzu.location)
            {
                if (kudzu.charge <= Act_Season_RazeKudzu.damageToKudzu)
                {
                    kudzu.location.properties.Remove(kudzu);
                    kudzu.location.properties.Add(new Pr_Season_PurgedKudzu(unit.location));
                }
                else
                {
                    kudzu.influences.Add(new ReasonMsg("Razing Army", -Act_Season_RazeKudzu.damageToKudzu));
                }

                unit.hp -= Act_Season_RazeKudzu.damageToArmy;
                if (unit.hp <= 0)
                    unit.die(unit.map, unit.getName() + " was wiped out driving back Dreaming Kudzu");
                unit.task = null;
            }
            else
            {
                if (unit.movesTaken > 0)
                {
                    return;
                }

                while (unit.movesTaken < unit.getMaxMoves())
                {
                    Location[] pathTo = unit.location.map.getPathTo(unit.location, kudzu.location, unit);
                    if (pathTo == null || pathTo.Length < 2)
                    {
                        unit.task = null;
                        break;
                    }

                    unit.location.map.adjacentMoveTo(unit, pathTo[1]);
                    unit.movesTaken++;
                    if (unit.location == kudzu.location)
                    {
                        break;
                    }
                }
            }
        }

        public override Location getLocation()
        {
            return kudzu.location;
        }


    }
}
