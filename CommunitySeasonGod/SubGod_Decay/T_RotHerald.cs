using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

namespace CommunitySeasonGod
{
    public class T_RotHerald : Trait
    {
        private SettlementHuman lastSettlement = null;

        public T_RotHerald() { }

        public override string getName() => "Rot Herald";

        public override string getDesc()
        {
            return $"While in a settlement, the base Food in their location is temporarily decreased by 5. If hunger or famine is present, it ticks up an additional 5% each turn.";
        }

        public override int getMaxLevel() => 1;

        public override void turnTick(Person p)
        {
            base.turnTick(p);

            if (lastSettlement != null && lastSettlement != p.unit.location.settlement)
                lastSettlement.foodLocal += 5;

            var settlement = (SettlementHuman)p.unit.location.settlement;
            if (settlement == null) return;

            // tick famine
            var famine = p.unit.location.properties.OfType<Pr_Famine>().FirstOrDefault();
            if (famine != null)
                Property.addToProperty("Trait: Rot Herald", Property.standardProperties.FAMINE, 5, settlement.location);

            if (lastSettlement != settlement)
            {
                settlement.foodLocal -= 5;
                lastSettlement = settlement;
            }
        }

        public override int[] getTags() => new int[0];
    }
}
