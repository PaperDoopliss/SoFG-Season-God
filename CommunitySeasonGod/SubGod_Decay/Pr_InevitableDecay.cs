using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Pr_InevitableDecay : Property
    {

        public Pr_InevitableDecay(Location loc) : base(loc) { }

        public override string getName() => DecayConsts.InevitableDecay;

        public override string getInvariantName() => DecayConsts.InevitableDecay;

        public override string getDesc()
        {
            return $"<b>{DecayConsts.Wilting}</b> accumulates twice as fast in this location.";
        }

        public override bool survivesRuin() => false;

        public override bool removedOnRuin() => true;

        public override Sprite getSprite(World world)
        {
            return base.getSprite(world);
        }

        public override bool hasBackgroundHexView()
        {
            return true;
        }

        public override Sprite getHexBackgroundSprite() => this.map.world.iconStore.ancientRuins;

        public Property WithCharges(double charges)
        {
            this.charge = charges;
            return this;
        }


    }
}
