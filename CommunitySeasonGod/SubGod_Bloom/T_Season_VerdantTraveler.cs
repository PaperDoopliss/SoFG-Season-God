using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class T_Season_VerdantTraveler : Trait
    {

        public override string getName()
        {
            return "Verdant Traveler";
        }

        public override string getDesc()
        {
            return "This person gains a Far Walk challenge they can perform at locations with Fey Crops, letting them instantly move to other locations with Fey Crops";
        }

        public override void onAcquire(Person person)
        {
            base.onAcquire(person);
            if (person.unit != null)
                person.unit.rituals.Add(new Rt_Season_FarWalk(person.unit.location));
        }

        public override void turnTick(Person p)
        {
            base.turnTick(p);
            Power farWalk = p.map.overmind.god.powers.FirstOrDefault(pow => pow is P_Season_FarWalk);
            if (farWalk != null)
                p.map.overmind.god.powers.Remove(farWalk);
        }

        public override void onDeath(Unit unit, Person killer)
        {
            base.onDeath(unit, killer);

            Power farWalk = unit.map.overmind.god.powers.FirstOrDefault(pow => pow is P_Season_FarWalk);
            if (farWalk != null)
                unit.map.overmind.god.powers.Remove(farWalk);
        }


    }
}
