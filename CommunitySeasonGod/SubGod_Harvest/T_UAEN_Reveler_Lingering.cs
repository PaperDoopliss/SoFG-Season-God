using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class T_UAEN_Reveler_Lingering : Trait
    {

        public int countdown = 15;
        public bool countdownStarted = true;
        public override string getName()
        {
            if(countdownStarted)
            {
                return "Lingering Reveler ("+countdown+")";
            }
            return "Lingering Reveler";
        }

        public override string getDesc()
        {
            if (countdownStarted)
            {
                return "This character is one that celebrates the Uncle of the Harvest and has been granted the energy to continue their festivities. They will linger on for 15 turns after the harvest season ends.\n\n" + countdown+" turns remaining.";
            }
            return "This character is one that celebrates the Uncle of the Harvest and has been granted the energy to continue their festivities. They will linger on for 15 turns after the harvest season ends.";
        }

        public override int getMaxLevel()
        {
            return 300;
        }

        public override bool preventDeathFromAge()
        {
            return true;
        }

        public override int[] getTags()
        {
            return new int[0];
        }

        public override void onAcquire(Person person)
        {
            base.onAcquire(person);
            if (person != null)
            {
                T_UAEN_Reveler reveler = person.traits.OfType<T_UAEN_Reveler>().FirstOrDefault();
                if (reveler != null)
                {
                    person.traits.Remove(reveler);
                }
            }
        }

        public override void turnTick(Person p)
        {
            base.turnTick(p);
            Map map = p.map;
            bool isHarvestSeason = false;
            if (map != null)
            {
                God_Season season = map.overmind.god as God_Season;
                if (season != null)
                {
                    if (season.ActiveSubGod.GetName() == "Uncle of the Harvest")
                    {
                        isHarvestSeason = true;
                    }
                }

                if (!isHarvestSeason)
                {
                    countdown--;
                    Unit u = p.unit;
                    if (u != null)
                    {
                        u.die(map, "Harvest season has ended, and so too have the revelries.");
                    }
                }
                else
                {
                    countdownStarted = false;
                    countdown = 15;
                }
            }
        }
    }
}
