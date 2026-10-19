using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class T_UAEN_Reveler : Trait
    {
        public override string getName()
        {
            return "Harvest Reveler";
        }

        public override string getDesc()
        {
            return "This character is one that celebrates the Uncle of the Harvest. They will cease to exist once the next season starts.";
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
                    Unit u = p.unit;
                    if (u != null)
                    {
                        u.die(map, "Harvest season has ended, and so too have the revelries.");
                    }
                }
            }
        }
    }
}
