using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Pr_Wilting : Property
    {

        public Pr_Wilting(Location loc) : base(loc) { }

        public override string getName() => DecayConsts.Wilting;

        public override string getInvariantName() => DecayConsts.Wilting;

        public override string getDesc()
        {
            return "The land grows heavy with rot and decay.";
        }

        public override string getCrisis()
        {
            return "When this mdofiers reaches 100%, the settlement is transformed into a Wilted Grove.";
        }

        public override void turnTick()
        {
            var plague = this.location.properties.FirstOrDefault(pr => pr.getPropType() == standardProperties.PLAGUE);
            var famine = this.location.properties.FirstOrDefault(pr => pr.getPropType() == standardProperties.FAMINE);

            if (!(this.location.settlement is SettlementHuman)) return;

            if (plague != null)
            {
                if (plague.charge > 100)
                    this.charge += 2;
                else
                    this.charge += 1;
            }

            if (famine != null)
            {
                if (famine.charge >= 100 && famine.charge < 200)
                    this.charge += 2;
                else
                    this.charge += Math.Max(2, (this.location.settlement as SettlementHuman).population * 0.05f);
            }


            if (this.charge == 100)
            {
                P_Season_AutumnsCaress.ConvertToWiltedGrove(this.location);
            }
        }

        public override bool survivesRuin() => true;

        public override bool removedOnRuin() => false;

        public override Sprite getSprite(World world)
        {
            return base.getSprite(world);
        }

        public override bool hasBackgroundHexView()
        {
            return true;
        }

        public override Sprite getHexBackgroundSprite() => this.map.world.iconStore.death;

        public Pr_Wilting WithCharges(int charges)
        {
            this.charge = charges;
            return this;
        }


    }
}
