using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class T_Season_WhispersOnTheWind : Trait
    {
        public T_Season_WhispersOnTheWind() {}

        public override string getName()
        {
            return "Whispers on the Wind";
        }

        public override string getDesc()
        {
            return "This person's location loses 1 <b>security</b> for every upwind location with at least some infiltration";
        }

        public override int getSecurityChange(Unit u, SettlementHuman settlementHuman)
        {
            int delta = 0;

            foreach (Location l in settlementHuman.location.getNeighbours())
            {

                if (l.settlement != null && l.settlement.infiltration > 0)
                {



                    foreach (Property pr in l.properties)
                    {

                        if (pr is Pr_Season_WindCurrent current)
                        {
                            if (current.downwind.Contains(settlementHuman.location))
                                delta--;
                        }
                    }
                }
            }
            

            return delta;
        }


    }
}
