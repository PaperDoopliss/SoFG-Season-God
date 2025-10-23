using Assets.Code;
using Assets.Code.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{

    public class Kernel_Season : ModKernel
    {

        private static Kernel_Season thisKernel;
        public static Kernel_Season get() { return thisKernel; }


        public int seasonLength = 80;

        public bool param_windEnabled = true;
        public bool param_huntEnabled = true;

        public override void onModsInitiallyLoaded()
        {
            thisKernel = this;
        }
        public override void beforeMapGen(Map map)
        {
            thisKernel = this;
        }
        public override void afterLoading(Map map)
        {
            thisKernel = this;
        }

        public override void onStartGamePresssed(Map map, List<God> gods)
        {
            gods.Add(new God_Season());
        }

        public override void receiveModConfigOpts_int(string optName, int value)
        {
            if (optName == "Season Length")
                thisKernel.seasonLength = value;
        }
        public override void receiveModConfigOpts_bool(string optName, bool value)
        {
            if (optName == "Master of the Hunt Enabled")
                thisKernel.param_huntEnabled = value;
            else if (optName == "Painter of Winds Enabled")
                thisKernel.param_windEnabled = value;
        }


    }
}
