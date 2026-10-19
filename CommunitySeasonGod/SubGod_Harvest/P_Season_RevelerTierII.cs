using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections.LowLevel.Unsafe;

namespace CommunitySeasonGod
{
    public class P_Season_RevelerTierII : P_Season
    {
        public P_Season_RevelerTierII(Map map) : base(map) { }

        public override string getName()
        {
            return "Second Revelers";
        }

        public override string getDesc()
        {
            return "Creates a Trickster Reveler who disrupts rulers and spreads unrest and fey presence while absorbing the menace of nearby agents. The higher the charge of the Fey Presence modifier used to cast this, the higher the Lore of the reveler, and the more menace they will take from your agents. Once the season changes, the Reveler will cease to exist.";
        }

        public override string getFlavour()
        {
            return "The first messengers of The Uncle’s mischievous nature are these lesser fey who slip into reality at the centre of local festivals. They share the Uncle’s fondness for pranks and showmanship and set forth to bring merriment to the world in the way they know best.";
        }
        public override string getRestrictionText()
        {
            return "Must be cast on a location with Fey Presence.";
        }
        public override int getCost()
        {
            return 3;
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
                UAEN_Season_HarvestRevelerTierII reveler = new UAEN_Season_HarvestRevelerTierII(loc, map.soc_dark, p);
                p.stat_lore = 1 + (int)presence.charge / 25;
                loc.units.Add(reveler);
                map.units.Add(reveler);
            }
        }
    }
}
