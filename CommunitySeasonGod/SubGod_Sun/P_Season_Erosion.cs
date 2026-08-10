using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_Erosion : P_Season
    {

        public static double devastationToInflict = 25;

        public P_Season_Erosion(Map map) : base(map) { }

        public override string getName()
        {
            return "Erosion";
        }

        public override string getDesc()
        {
            return "Increases a location's <b>devastation</b> by " + devastationToInflict + "%";
        }


        public override string getRestrictionText()
        {
            return "Must target a populated settlement";
        }

        public override string getFlavour()
        {
            return "The heat leaves materials brittle, crops withered and people exhausted. Crumbling is inevitable.";
        }


        public override bool validTarget(Unit unit)
        {
            return false;
        }


        public override bool validTarget(Location loc)
        {
            if (loc.settlement is SettlementHuman)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int getCost()
        {
            return 1;
        }

        public override void cast(Location loc)
        {
            base.cast(loc);
            Property.addToPropertySingleShot(getName(), Property.standardProperties.DEVASTATION, devastationToInflict, loc);

        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_erosion.png");
        }



    }
}
