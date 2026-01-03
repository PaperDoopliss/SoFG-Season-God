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

        public Pr_Wilting(Location loc) : base(loc) {
            this.charge = 0.0001;
        }

        public override string getName() => DecayConsts.Wilting;

        public override string getInvariantName() => DecayConsts.Wilting;

        public override string getDesc()
        {
            return @"The land grows heavy with rot and decay.

Increases by 1% each time population in the settlement is reduced by plague and 2% when reduced by famine.

Decays by 0.5% each turn if <b>fey presence</b> is below 50%.";
        }

        public override string getCrisis()
        {
            return "When this modifier reaches 100%, the settlement is transformed into a Wilted Grove.";
        }

        public override void turnTick()
        {
            SubGod_Decay.GeneratePassiveWilting(this.location);

            var fey = this.location.properties.OfType<Pr_FeyPresence>().FirstOrDefault();

            if (fey != null && fey.charge >= 50) return;

            this.influences.Add(new ReasonMsg("Fey Presence Below 50%", -0.05f));
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

        public Pr_Wilting WithCharges(double charges)
        {
            this.charge = charges;
            return this;
        }


    }
}
