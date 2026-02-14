//using Assets.Code;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace CommunitySeasonGod
//{
//    public class Pr_GrainBlight : Property
//    {

//        public Pr_GrainBlight(Location loc) : base(loc) {
//            this.charge = 0.0001;
//        }

//        public override string getName() => DecayConsts.Wilting;

//        public override string getInvariantName() => DecayConsts.Wilting;

//        public override string getDesc()
//        {
//            return @"The location's habitability is reduced by this modifiers value.";
//        }

//        public override void turnTick()
//        {
//            SubGod_Decay.GeneratePassiveWilting(this.location);

//            var fey = this.location.properties.OfType<Pr_FeyPresence>().FirstOrDefault();

//            if (fey != null && fey.charge >= 50) return;

//            this.influences.Add(new ReasonMsg("Fey Presence Below 50%", -0.05f));
//        }

//        public override bool survivesRuin() => true;

//        public override bool removedOnRuin() => false;

//        public override Sprite getSprite(World world)
//        {
//            return base.getSprite(world);
//        }

//        public override bool hasBackgroundHexView()
//        {
//            return true;
//        }

//        public override Sprite getHexBackgroundSprite() => this.map.world.iconStore.death;

//        public Pr_Wilting WithCharges(double charges)
//        {
//            this.charge = charges;
//            return this;
//        }


//    }
//}
