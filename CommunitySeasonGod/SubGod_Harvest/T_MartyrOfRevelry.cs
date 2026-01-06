using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    internal class T_MartyrOfRevelry : Trait
    {
        public override string getName()
        {
            return "Martyr of Revelry";
        }

        public override string getDesc()
        {
            return "The Supplicant is well beloved by the people for their joyful and mischevious ways. The death of the Supplicant enrages the people and creates unrest";
        }

        public override int getMaxLevel()
        {
            return 1;
        }

        public override void onDeath(Unit unit, Person killer)
        {
            if (unit.location.settlement is SettlementHuman)
            {

                foreach (Property property in unit.location.properties)
                {
                    if (property is Pr_Unrest pr_Unrest)
                    {
                        pr_Unrest.influences.Add(new ReasonMsg("Martyr of Revelry", 150));
                        return;
                    }
                }

                Pr_Unrest pr_UnrestMake = new Pr_Unrest(unit.location);
                pr_UnrestMake.charge = 150;
                unit.location.properties.Add(pr_UnrestMake);
            }
        }

        public override int[] getTags()
        {
            return new int[0];
        }
    }
}
