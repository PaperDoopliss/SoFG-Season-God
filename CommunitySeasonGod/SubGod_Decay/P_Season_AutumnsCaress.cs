using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_AutumnsCaress : P_Season
    {
        public P_Season_AutumnsCaress(Map map) : base(map) { }

        public override string getName() => "Autumn's Caress";

        public override string getDesc()
        {
            return @"Tranforms a settlement location with <b>autumnal rot</b> greater than its population into a <b>wilting grove</b>.

Wilting groves periodically spawn armies of Wilted Hordes to harass nearby settlements.

The size of the new grove is based on the population of the city it was created from.";
        }

        public override string getFlavour()
        {
            return "As famine sets in, the bodies begin to pile up, raked from the streets into great mounds that grow into pretty groves of vibrant red, yellow and orange. Prettier by far than the drab cobblestones, the sunken grief stricken townsfolk, until the lines between mass grave and paradise begin to blur. And that is when the lady visits them, guiding each towards those pretty gardens in turn, promising an end to their toil.";
        }

        public override string getRestrictionText()
        {
            return $"Must target a settlement with a greater <b>{DecayConsts.Wilting.ToLower()}</b> modifier than its current population.";
        }

        public override Sprite getIconFore()
        {
            return map.world.iconStore.foreverDying;
        }

        public override bool validTarget(Unit unit)
        {
            return false;
        }

        public override bool validTarget(Location loc)
        {
            if (!(loc.settlement is SettlementHuman)) return false;
            var settlement = (SettlementHuman)loc.settlement;
            var decay = loc.properties.FirstOrDefault(p => p.getInvariantName() == DecayConsts.Wilting);
            if (decay != null && settlement.population <= decay.charge) return true;
            return false;
        }

        public override int getCost() => 0;

        public override void cast(Location loc)
        {
            base.cast(loc);
            if (loc.settlement != null && loc.settlement is SettlementHuman)
            {
                ConvertToWiltedGrove(loc);
            }
        }

        public static void ConvertToWiltedGrove(Location loc)
        {
            if (loc.settlement == null || loc.settlement is SettlementHuman) return;
            int size = (loc.settlement as SettlementHuman).population;
            loc.settlement = new Set_WiltedGrove(loc, size);
            loc.soc = new SG_AgentDark(loc.map);
        }

    }
}
