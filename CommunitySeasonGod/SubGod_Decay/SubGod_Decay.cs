using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Assets.Code.Property;

namespace CommunitySeasonGod
{
    public class SubGod_Decay : SubGod
    {
        public SubGod_Decay(God_Season god, Map map)
            : base (god, map)
        {
            // Powers.Add(new P_Season_HuntCamouflage(map));
            PowerLevelReqs.Add(0);
            // BonusPowers.Add(new P_Season_HuntBonus(map));
            BonusPowerLevelReqs.Add(0);
        }

        public override string GetName()
        {
            return "The Lady of Wilting Leaves";
        }

        public override string GetKeywords()
        {
            return "Famine, Recycling, Armies";
        }

        //public override string GetEventPath()
        //{
        //    return "ComSeasonGod.shift_hunt";
        //}

        //public override void FeyPresenceTurnTick(Property fey)
        //{
        //    var wilting = fey.location.properties.OfType<Pr_Wilting>().FirstOrDefault();

        //    if (wilting == null) GeneratePassiveWilting(fey.location);
        //}

        public override bool HasSupplicantStartingTraits() => true;

        public override List<Trait> GetSupplicantStartingTraits()
        {
            return new List<Trait> { new T_RotHerald() };
            ;
        }

        public override string GetSpritePath()
        {
            return "ComSeasonGod.portrait_hunt.png";
        }

        public override void TurnTick_Active(Map map)
        {
            base.TurnTick_Active(map);
            foreach (Location loc in map.locations.Where(l => l.settlement != null && l.settlement is SettlementHuman))
            {
                if (loc.properties.OfType<Pr_Wilting>().FirstOrDefault() == null) GeneratePassiveWilting(loc);
            }

            foreach (var agent in map.persons.Where(p => p.unit is UAE))
            {
                if (agent.unit.rituals.OfType<Rt_GrainBlight>().FirstOrDefault() == null)
                    agent.unit.rituals.Add(new Rt_GrainBlight(p.unit.location));
                if (agent.unit.rituals.OfType<Rt_HeraldInTheFall>().FirstOrDefault() == null)
                    agent.unit.rituals.Add(new Rt_HeraldInTheFall(p.unit.location));
            }
        }

        public static void GeneratePassiveWilting(Location location)
        {
            var plague = location.properties.OfType<Pr_Plague>().FirstOrDefault();
            var famine = location.properties.OfType<Pr_Famine>().FirstOrDefault();
            var inevitableDecay = location.properties.OfType<Pr_InevitableDecay>().FirstOrDefault();
            var wilting = location.properties.OfType<Pr_Wilting>().FirstOrDefault();

            if (!(location.settlement is SettlementHuman)) return;

            double charges = 0;

            if (plague != null)
            {
                if (plague.charge > 100)
                    charges += 2;
                else
                    charges += 1;
            }

            if (famine != null)
            {
                if (famine.charge >= 100 && famine.charge < 200)
                    charges += 4;
                else
                    charges += Math.Max(2, (location.settlement as SettlementHuman).population * 0.05f) * 2;
            }

            if (inevitableDecay != null) charges *= 2;

            if (charges == 0) return;

            if (wilting == null)
            {
                location.properties.Add(new Pr_Wilting(location).WithCharges(charges));
            }
            else
                wilting.influences.Add(new ReasonMsg("Plague and Famine casualities", charges));

            if (wilting == null) return;

            if (wilting.charge == 100)
            {
                P_Season_AutumnsCaress.ConvertToWiltedGrove(location);
            }
        }

    }
}
