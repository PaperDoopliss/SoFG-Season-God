using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_Typhoon : P_Season
    {
        public P_Season_Typhoon(Map map) : base(map) { }

        public static double chargeCost = 50;
        public static double devastation = 200;

        public override string getName()
        {
            return "Typhoon";
        }

        public override string getDesc()
        {
            return "Spend " + chargeCost + "% Fey Presence at a coastal location to inflict " + devastation + "% <b>devastation</b>.";
        }

        public override string getFlavour()
        {
            return "Coastal peoples learn from a young age that, if the winds want to hurt you, they will.";
        }

        public override string getRestrictionText()
        {
            return "Must target a populated coastal settlement with at least " + chargeCost + "% Fey Presence";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_typhoon.png");
        }

        public override bool validTarget(Location loc)
        {
            if (loc.settlement is SettlementHuman)
            {
                if (loc.isCoastal)
                {
                    foreach (Property pr in loc.properties)
                    {
                        if (pr is Pr_FeyPresence && pr.charge >= chargeCost)
                            return true;
                    }
                }
            }

            return false;
        }

        public override int getCost()
        {
            return 4;
        }

        public override void cast(Location location)
        {
            base.cast(location);

            Property.addToPropertySingleShot(getName(), Property.standardProperties.DEVASTATION, devastation, location);

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
