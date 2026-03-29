using Assets.Code;
using CommunitySeasonGod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class T_Season_LordOfTheFeast : Trait
    {

        public T_Season_LordOfTheFeast() : base()
        {

        }

        public override string getName()
        {
            return "Lord of the Feast";
        }

        public override string getDesc()
        {
            return "This person is an alter ego of the Lord of the Feast, embodied so he can walk among humanity and appreciate their culture. If this body is destroyed, the season will change, and the Lord of the Feast can no longer be selected.";
        }

        public override void onDeath(Unit unit, Person killer)
        {
            base.onDeath(unit, killer);

            if (unit.map.overmind.god is God_Season season)
            {

                if (season.removeSubGod(typeof(SubGod_Feast)))
                {
                    if (!EventManager.events.TryGetValue("ComSeasonGod.pale_knight_dead", out EventManager.ActiveEvent ae))
                    {
                        Console.WriteLine("ComSeasonGod: Unable to find Pale Knight Dead event (\"ComSeasonGod.pale_knight_dead\").");
                        unit.map.addMessage("ERROR: Unable to find Pale Knight Dead event (\"ComSeasonGod.pale_knight_dead\").", 1.0, false);
                        return;
                    }

                    unit.map.world.prefabStore.popEvent(ae.data, EventContext.withNothing(unit.map), null, false);
                }

            }
        }



    }
}
