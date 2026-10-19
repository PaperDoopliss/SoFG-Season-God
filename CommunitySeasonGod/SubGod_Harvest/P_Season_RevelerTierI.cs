using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections.LowLevel.Unsafe;

namespace CommunitySeasonGod
{
    public class P_Season_RevelerTierI : P_Season
    {
        public P_Season_RevelerTierI(Map map) : base(map) { }

        public override string getName()
        {
            return "First Revelers";
        }

        public override string getDesc()
        {
            return "Creates a Jolly Reveler who will convert a location's Unrest and Devastation into Fey Presence while absorbing the profile of nearby agents. The higher the charge of the Fey Presence modifier, the higher the Lore of the reveler, and the more profile they will take from your agents. Once the season changes, the Reveler will cease to exist.";
        }

        public override string getFlavour()
        {
            return "The first messengers of The Uncle’s merriment to the world are not fey but mortals. These scions have been feytouched, taught the joys of sharing the Harvest’s bounty and set forth to teach others.";
        }
        public override string getRestrictionText()
        {
            return "Must be cast on a location with Fey Presences.";
        }
        public override int getCost()
        {
            return 2;
        }
        public override bool validTarget(Location loc)
        {
            foreach (Property prop in loc.properties)
            {

                if (prop is Pr_FeyPresence)
                {
                    return true;
                }
            }

            return false;
        }

        public override void cast(Location loc)
        {
            base.cast(loc);
            Pr_FeyPresence presence = loc.properties.OfType<Pr_FeyPresence>().FirstOrDefault();
            if (presence != null)
            {
                Person p = new Person(map.soc_dark);
                UAEN_Season_HarvestRevelerTierI reveler = new UAEN_Season_HarvestRevelerTierI(loc, map.soc_dark, p);
                p.stat_lore = 1 + (int)presence.charge / 25;
                loc.units.Add(reveler);
                map.units.Add(reveler);
            }
        }
    }
}
