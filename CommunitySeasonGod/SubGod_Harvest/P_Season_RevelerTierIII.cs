using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections.LowLevel.Unsafe;

namespace CommunitySeasonGod
{
    public class P_Season_RevelerTierIII : P_Season
    {
        public P_Season_RevelerTierIII(Map map) : base(map) { }

        public override string getName()
        {
            return "Eternal Revelries";
        }

        public override string getDesc()
        {
            return "Creates a Lord Reveler who spreads Eternal Revelry and who passively absorbs the profile and menace of nearby agents. The higher the charge of the Fey Presence modifier used to cast this, the higher the Lore of the reveler, and the more potent they become.";
        }

        public override string getFlavour()
        {
            return "The first messengers of The Uncle’s power are these fey lords who entice mortals into revelries like the Uncle’s own. Their joyous carefree nature is intoxicating, drawing mortals to join in. As a statement of the Uncle’s own strength they even withstand the passing of seasons longer than they should.";
        }
        public override string getRestrictionText()
        {
            return "Must be cast on a location with Fey Presence.";
        }
        public override int getCost()
        {
            return 4;
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
                UAEN_Season_HarvestRevelerTierIII reveler = new UAEN_Season_HarvestRevelerTierIII(loc, map.soc_dark, p);
                p.stat_lore = 1 + (int)presence.charge / 25;
                loc.units.Add(reveler);
                map.units.Add(reveler);
            }
        }
    }
}
