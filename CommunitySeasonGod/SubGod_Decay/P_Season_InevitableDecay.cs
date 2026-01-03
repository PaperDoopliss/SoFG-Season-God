using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_InevitableDecay : P_Season
    {
        public P_Season_InevitableDecay(Map map) : base(map) { }

        public override string getName() => "Inevitable Decay";

        public override string getDesc()
        {
            return $"Cast on a settlement location to create 100% <b>inevitable decay</b>. Inevitable decay is reduced by 5% each turn, and causes <b>{DecayConsts.Wilting.ToLower()}</b> to be generated twice as fast.";
        }

        public override string getFlavour()
        {
            return "...";
        }

        public override string getRestrictionText()
        {
            return $"Target location must be a settlement.";
        }

        public override Sprite getIconFore()
        {
            return map.world.iconStore.ancientRuins;
        }

        public override bool validTarget(Unit unit)
        {
            return false;
        }

        public override bool validTarget(Location loc)
        {
            if (!(loc.settlement is SettlementHuman)) return false;
            return true;
        }

        public override int getCost() => 2;

        public override void cast(Location loc)
        {
            base.cast(loc);
            Pr_InevitableDecay invDecay = loc.properties.OfType<Pr_InevitableDecay>().FirstOrDefault();

            if (invDecay == null)
            {
                loc.properties.Add(invDecay.WithCharges(100));
            }
            else if (invDecay != null) {
                invDecay.charge += 100;
            }
        }

    }
}
