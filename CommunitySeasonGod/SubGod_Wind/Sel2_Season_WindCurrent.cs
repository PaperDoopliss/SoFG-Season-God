using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class Sel2_Season_WindCurrent : SelectClickReceiver
    {

        public Map map;
        public Location target;

        public Sel2_Season_WindCurrent(Map map, Location target)
        {
            this.map = map;
            this.target = target;
        }

        public void cancelled()
        {
            map.overmind.power += P_Season_WindCurrent.cost;
        }

        public void selectableClicked(string text, int index)
        {

            Pr_Season_WindCurrent.windCurrentDirection direction = Pr_Season_WindCurrent.windCurrentDirection.All;
            if (text == "Northwest")
                direction = Pr_Season_WindCurrent.windCurrentDirection.NW;
            else if (text == "Northeast")
                direction = Pr_Season_WindCurrent.windCurrentDirection.NE;
            else if (text == "Southeast")
                direction = Pr_Season_WindCurrent.windCurrentDirection.SE;
            else if (text == "Southwest")
                direction = Pr_Season_WindCurrent.windCurrentDirection.SW;
            else if (text == "All")
                direction = Pr_Season_WindCurrent.windCurrentDirection.SE;

            Pr_Season_WindCurrent current = null;
            foreach (Property pr in target.properties)
            {
                if (pr is Pr_Season_WindCurrent foundCurrent)
                {
                    current = foundCurrent;
                    current.direction = direction;
                    current.updateDownwindLocations();
                    current.effect = Pr_Season_WindCurrent.windCurrentEffect.NONE;
                    map.overmind.power += P_Season_WindCurrent.cost;
                    break;
                }
            }
            if (current == null)
            {
                current = new Pr_Season_WindCurrent(target, direction);
                current.charge = 100;
                target.properties.Add(current);
            }

            GraphicalMap.checkData();
            map.world.ui.checkData();
        }



    }
}
