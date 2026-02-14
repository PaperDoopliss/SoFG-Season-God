using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Rt_HeraldInTheFall : Ritual
    {
        private const int MODIFIER = 25; 

        public Rt_HeraldInTheFall(Location loc)
          : base(loc)
        {
        }

        public override string getName() => "Herald in the Fall";

        public override string getDesc()
        {
            return $"Generates {MODIFIER} <b>devastation</b> in this location, which harms prosperity and food production. <b>Complexity</b> increases with location <b>Security</b> and decreases with <b>fey presence</b>. Devastated cities suffer <b>hunger</b>, which is a good source of <b>{DecayConsts.Wilting.ToLower()}</b>.";
        }

        public override string getRestriction()
        {
            return "Requires a fully Infiltrated Human Settlement.";
        }

        public override string getCastFlavour()
        {
            return "When the lady's power is brought to bare, grain turns to waste in the silos and fruit rots on abandoned market stalls. The piling dead mulch for the lucky survivor's salvation.";
        }

        public override Sprite getSprite() => this.map.world.iconStore.famine;

        public override double getProfile() => 5.0;

        public override double getMenace() => 20.0;

        public override Challenge.challengeStat getChallengeType() => Challenge.challengeStat.LORE;

        public override void complete(UA u)
        {
            Property.addToPropertySingleShot(u.getName(), Property.standardProperties.DEVASTATION, MODIFIER, this.location);
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            msgs?.Add(new ReasonMsg("Stat: Lore", (double)Math.Max(1, unit.getStatLore())));
            return (double)Math.Max(1, unit.getStatIntrigue());
        }

        public override double getComplexity()
        {
            var fey = this.location.properties.OfType<Pr_FeyPresence>().FirstOrDefault();
            var feyBonus = fey.charge / 2;

            return Math.Max(1, 25 + this.map.param.ch_complexityPerSecurityPoint - fey.charge);
        }

        public override bool valid()
        {
            return this.location.settlement is SettlementHuman settlement && this.location.settlement.infiltration == 100;
        }
    }
}
