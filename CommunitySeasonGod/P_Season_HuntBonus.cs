using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_HuntBonus : P_Season_LimitedCharges
    {
        public P_Season_HuntBonus(Map map) : base(map) { }

        public override string getName()
        {
            return "Hunter's Blessing";
        }

        public override string getDesc()
        {
            return "Grants an agent +1 to all stats while this Noble is active. Can only be used once.";
        }

        public override string getFlavour()
        {
            return "THIS IS SUPER PLACEHOLDER, I JUST NEED POWERS FOR TESTING PURPOSES";
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

        //Not done yet
        public override void cast(Unit u)
        {
            base.cast(u);
            if (u is UA uA)
            {
                uA.setProfile(uA.profile - 10);
                uA.setMenace(uA.menace - 10);
            }
        }


    }
}
