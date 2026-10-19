using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_HarvestVeilbreaker : P_Season
    {
        public P_Season_HarvestVeilbreaker(Map map) : base(map) { }


        public override string getName()
        {
            return "Veil Breaker";
        }

        public override string getDesc()
        {
            return "Creates a new Geomantic locus with 100% charge";
        }

        public override Sprite getIconFore()
        {
            return map.world.iconStore.ophanimLight;
        }

        public override bool validTarget(Unit unit)
        {
            return false;
        }

        public override bool validTarget(Location loc)
        {
            if (loc.settlement == null)
            {
                foreach (Property property in loc.properties)
                {
                    if (property is Pr_GeomanticLocus)
                    {
                        return false;
                    }
                }

                return true;
            }
            if (loc.settlement is SettlementHuman)
            {
                if (loc.settlement.infiltration == 0.0)
                {
                    return false;
                }

                foreach (Property property in loc.properties)
                {
                    if (property is Pr_GeomanticLocus)
                    {
                        return false;
                    }
                }

                return true;
            }

            foreach (Property property in loc.properties)
            {
                if (property is Pr_GeomanticLocus)
                {
                    return false;
                }
            }

            return true;
        }

        public override int getCost()
        {
            return 5;
        }

        public override void cast(Location loc)
        {
            base.cast(loc);
            Pr_GeomanticLocus pr_Opha_Faith = new Pr_GeomanticLocus(loc);
            pr_Opha_Faith.charge = 100.0;
            loc.properties.Add(pr_Opha_Faith);
        }

        public override string getFlavour()
        {
            return "A rip, a gash, a tear in reality. Raw magical power and sounds of raucus merriement seep through from beyond.";
        }

        public override string getRestrictionText()
        {
            return "Must be cast on an uninhabited location or a human settlement with infiltration > 0%";
        }
    }
}

