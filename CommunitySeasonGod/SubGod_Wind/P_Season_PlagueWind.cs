using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_PlagueWind : P_Season
    {
        public P_Season_PlagueWind(Map map) : base(map) { }

        public static double chargeCost = 100;
        public static double plague = 100;

        public override string getName()
        {
            return "Plague Wind";
        }

        public override string getDesc()
        {
            return "Spend " + chargeCost + "% Fey Presence at a populated location to inflict " + plague + "% <b>plague</b>.";
        }

        public override string getFlavour()
        {
            return "The Painter whips ambient diseases into a frenzy, every particle of illness unerringly finding a host.";
        }

        public override string getRestrictionText()
        {
            return "Must target a populated settlement with at least " + chargeCost + "% Fey Presence";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_plague_wind.png");
        }

        public override bool validTarget(Location loc)
        {
            if (loc.settlement is SettlementHuman)
            {
                foreach (Property pr in loc.properties)
                {
                    if (pr is Pr_FeyPresence && pr.charge >= chargeCost)
                        return true;
                }
                
            }

            return false;
        }

        public override int getCost()
        {
            return /*1*/0;
        }

        public override void cast(Location location)
        {
            base.cast(location);

            Property.addToPropertySingleShot(getName(), Property.standardProperties.PLAGUE, plague, location);

            foreach (Property pr in location.properties)
            {
                if (pr is Pr_FeyPresence)
                {
                    pr.charge -= chargeCost;
                    return;
                }
            }
        }


    }
}
