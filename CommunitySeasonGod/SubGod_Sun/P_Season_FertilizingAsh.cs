using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_FertilizingAsh : P_Season
    {
        public static int xpPerAsh = 3;

        public P_Season_FertilizingAsh (Map map) : base(map) { }

        public override string getName()
        {
            return "Fertilizing Ash";
        }

        public override string getDesc()
        {
            return "Consumes Ashen Earth in an agent's location and its neighbours to grant the agent " + xpPerAsh + " <b>xp</b> per percentage consumed";
        }

        public override string getFlavour()
        {
            return "The ruin and death spread by the Patriarch settles into the ash left in his wake, waiting to be consumed by his earthly tools.";
        }

        public override string getRestrictionText()
        {
            return "Must target an agent who is in or next to a location with Ashen Earth";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_fertilizing_ash.png");
        }

        public override bool validTarget(Unit unit)
        {
            if (unit is UA == false || unit.isCommandable() == false || unit.person == null)
                return false;

            foreach (Property pr in unit.location.properties)
            {
                if (pr is Pr_Season_AshenEarth)
                    return true;
            }
            foreach (Location l in unit.location.getNeighbours())
            {
                foreach (Property pr in l.properties)
                {
                    if (pr is Pr_Season_AshenEarth)
                        return true;
                }
            }

            return false;
        }

        public override bool validTarget(Location loc)
        {
            return false;
        }

        public override int getCost()
        {
            return 1;
        }

        public override void cast(Unit unit)
        {
            base.cast(unit);

            for (int i = 0; i < unit.location.properties.Count; i++)
            {
                if (unit.location.properties[i] is Pr_Season_AshenEarth ash)
                {
                    unit.person.receiveXP(Convert.ToInt32(ash.charge) * xpPerAsh);
                    unit.location.properties.RemoveAt(i);
                    i--;
                }
            }

            foreach (Location l in unit.location.getNeighbours())
            {
                for (int i = 0; i < l.properties.Count; i++)
                {
                    if (l.properties[i] is Pr_Season_AshenEarth ash)
                    {
                        unit.person.receiveXP(Convert.ToInt32(ash.charge) * xpPerAsh);
                        l.properties.RemoveAt(i);
                        i--;
                    }
                }
            }
        }


    }
}