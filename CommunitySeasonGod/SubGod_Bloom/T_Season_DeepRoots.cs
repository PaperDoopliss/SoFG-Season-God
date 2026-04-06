using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class T_Season_DeepRoots : Trait
    {
        Person person = null;

        public override string getName()
        {
            return "Deep Roots";
        }

        public override string getDesc()
        {
            return "This person cannot move from their home location";
        }

        public override void onAcquire(Person person)
        {
            base.onAcquire(person);

            this.person = person;
        }

        public override void turnTick(Person p)
        {
            base.turnTick(p);

            if (p.unit != null)
            {

                p.unit.movesTaken = p.unit.getMaxMoves();
                if (p.unit.location != p.map.locations[p.unit.homeLocation])
                {
                    p.unit.location.units.Remove(p.unit);
                    p.map.locations[p.unit.homeLocation].units.Add(p.unit);
                    p.unit.location = p.map.locations[p.unit.homeLocation];
                }
            }
        }

        public override void onMove(Location current, Location dest)
        {
            base.onMove(current, dest);

            if (person?.unit != null)
            {
                current.units.Remove(person.unit);
                dest.units.Remove(person.unit);
                person.unit.location = person.map.locations[person.unit.homeLocation];
                person.map.locations[person.unit.homeLocation].units.Add(person.unit);
                person.unit.task = null;


            }
        }


    }
}
