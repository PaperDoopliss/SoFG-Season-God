using Assets.Code;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{

    public class P_Season_EnnobleSavage : P_Season
    {
        public P_Season_EnnobleSavage(Map map) : base(map) { }


        public override string getName()
        {
            return "Ennoble Savage";
        }

        public override string getDesc()
        {
            return "Transforms an Orc Upstart into a Great Orc, increasing their stats and driving them to raid neighbours more aggressively. If they are in a location with at least " + T_Season_GreatOrc.amountRequired + "% Fey Presence, they will abort their previous task to transform that Fey Presence into an army of equal strength.";
        }

        public override string getFlavour()
        {
            return "The Lord of the Feast imposes his twisted vision of civilization onto the Orc, creating what could only look to him like a superior life form.";
        }

        public override string getRestrictionText()
        {
            return "Must target an Orc Upstart";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_ennoble_savage.png");
        }

        public override bool validTarget(Unit unit)
        {
            if (unit is UAEN_OrcUpstart)
            {
                if (unit is UAEN_Season_GreatOrc)
                    return false;
                return true;
            }

            return false;
        }

        public override int getCost()
        {
            return 2;
        }

        public override void cast(Unit unit)
        {
            base.cast(unit);

            Person p = unit.person;
            if (p != null)
            {
                map.units.Remove(unit);
                unit.location.units.Remove(unit);

                UAEN_Season_GreatOrc greatOrc = new UAEN_Season_GreatOrc(unit.location, unit.society, p);
                p.stat_might++;
                p.stat_command++;
                p.stat_intrigue++;
                p.stat_lore++;
                greatOrc.inner_menace = unit.inner_menace;
                greatOrc.inner_profile = unit.inner_profile;

                greatOrc.location.units.Add(greatOrc);
                map.units.Add(greatOrc);

                if (GraphicalMap.selectedUnit == unit)
                    GraphicalMap.selectedUnit = greatOrc;
            }
        }


    }

}
