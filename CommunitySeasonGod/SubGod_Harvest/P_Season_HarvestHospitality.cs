using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_HarvestHospitality : P_Season
    {
        public P_Season_HarvestHospitality(Map map) : base(map) { }

        public override string getName()
        {
            return "Law of Hospitality";
        }

        public override string getDesc()
        {
            return "Reduces an agent's <b>profile</b> and <b>menace</b> by 10, up to their minimums.";
        }

        public override string getFlavour()
        {
            return "Since ancient times the laws of hospitality have been absolute, when a wanderer approaches your door looking for shelter you shelter them so as to not anger the spirits.";
        }

        public override string getRestrictionText()
        {
            return "Must target an agent under your control";
        }

        public override Sprite getIconFore()
        {
            return map.world.iconStore.banditry;
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
            return 1;
        }

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

