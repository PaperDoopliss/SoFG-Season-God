using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class T_VeilPiercer : Trait
    {
        public override string getName()
        {
            return "Veil Piercer";
        }

        public override string getDesc()
        {
            return "The supplicant's precence pierces the veil between worlds, increasing the strength of local geomantic loci.";
        }

        public override int getMaxLevel()
        {
            return 1;
        }

        public override void turnTick(Person p)
        {
            foreach (Property property in p.unit.location.properties)
            {
                if (property is Pr_GeomanticLocus pr_GeomanticLocus)
                {
                    pr_GeomanticLocus.influences.Add(new ReasonMsg("Veil Piercer", 2.0));
                    break;
                }
            }
        }

        public override int[] getTags()
        {
            return new int[0];
        }
    }
}
