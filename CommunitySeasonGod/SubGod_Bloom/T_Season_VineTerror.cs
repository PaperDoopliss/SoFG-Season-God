using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class T_Season_VineTerror : Trait
    {
        public override string getName()
        {
            return "Creeping Monstrosity";
        }

        public override string getDesc()
        {
            return "This creature will move to a random populated settlement and attempt to root in the ground. If it succeeds, the location gains " + Task_Season_VineTerrorRoot.shadowToAdd * 100 + "% <b>shadow</b> and " + Task_Season_VineTerrorRoot.presenceToAdd + "% Fey Presence.";
        }



    }
}
