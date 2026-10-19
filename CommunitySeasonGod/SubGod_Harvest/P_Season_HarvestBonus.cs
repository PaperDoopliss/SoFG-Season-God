using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_HarvestBonus : P_Season_LimitedCharges
    {
        public P_Season_HarvestBonus(Map map)
            : base(map)
        {

        }

        public override string getName()
        {
            return "First Harvest";
        }

        public override string getDesc()
        {
            return "Grants one level to an Agent. Can only be used once";
        }

        public override string getFlavour()
        {
            return "The first of a well tended harvest always tastes the best and so The Uncle of the Harvest grants them to the loyal agents the family has left him.";
        }

        public override string getRestrictionText()
        {
            return "Must target an agent under your control";
        }

        public override Sprite getIconFore()
        {
            return map.world.iconStore.give;
        }

        public override bool validTarget(Unit unit)
        {
            if (unit is UA uA && uA.isCommandable())
                return true;

            return false;
        }

        public override bool validTarget(Location loc)
        {
            return false;
        }

        public override int getCost()
        {
            return 0;
        }


        public override void cast(Unit u)
        {
            base.cast(u);
            u.person.receiveXP(u.person.XPForNextLevel);
            SpendCharge();
        }


    }
}
