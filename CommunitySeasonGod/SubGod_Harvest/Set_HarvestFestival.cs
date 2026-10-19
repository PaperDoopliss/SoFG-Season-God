using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Set_HarvestFestival : Settlement
    {
        public Set_HarvestFestival(Location loc) : base(loc)
        {
            subs.Add(new Sub_HarvestFestival(this));
            shadow = 1;
            shadowPolicy = shadowResponse.FULL_FLOW;
        }

        public List<Location> getAffected()
        {
            List<Location> list = new List<Location>();

            list.Add(base.location); // this will include the target location

            foreach (Location neighbour in base.location.getNeighbours())
            {
                list.Add(neighbour);
            }

            return list;
        }

        public override void turnTick()
        {
            base.turnTick();
            List<Location> affected = getAffected();

            foreach (Location loc in affected)
            {
                if (loc.settlement != null)
                {
                    loc.settlement.shadow = Math.Min(1, loc.settlement.shadow + 0.1);
                }

                Pr_FeyPresence existing = null; // check if it already has feypresence

                foreach (Property property in loc.properties)
                {
                    if (property is Pr_FeyPresence)
                    {
                        existing = (Pr_FeyPresence)property; // grab that instance of feypresence
                        break; // stop checking other properties at this point
                    }
                }

                if (existing != null) // which we determined earlier
                {
                    existing.influences.Add(new ReasonMsg("Revelry", 2)); // add to that feypresence we grabbed
                }
                else // allows a separate logic for 'if none exists'
                {
                    Pr_FeyPresence pr_FeyPresence = new Pr_FeyPresence(loc); // create new one
                    pr_FeyPresence.charge = 2;
                    loc.properties.Add(pr_FeyPresence);
                }
            }

            if (map.turn % 10 == 0) //going by orc upstart spawn logic here, if the map turn is divisable by 10, spawn a random reveler
            {
                Location loc = this.location;
                int randomNum = Eleven.random.Next(10);

                if (randomNum % 2 == 0) // even numbers gets us a jolly reveler
                {
                    Person p = new Person(map.soc_dark);
                    UAEN_Season_HarvestRevelerTierI reveler = new UAEN_Season_HarvestRevelerTierI(loc, map.soc_dark, p);
                    p.stat_lore = 5;
                    p.receiveTrait(new T_UAEN_Reveler_Lingering());
                    loc.units.Add(reveler);
                    map.units.Add(reveler);
                }
                else //odd gives us trickster revelers
                {
                    Person p = new Person(map.soc_dark);
                    UAEN_Season_HarvestRevelerTierII reveler = new UAEN_Season_HarvestRevelerTierII(loc, map.soc_dark, p);
                    p.stat_lore = 5;
                    p.receiveTrait(new T_UAEN_Reveler_Lingering());
                    loc.units.Add(reveler);
                    map.units.Add(reveler);
                }
            }
        }
    }
}
