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

        private static Kernel_Season kernel;

        public static Kernel_Season Instance
        {
            get
            {
                return kernel;
            }
        }


        public int opt_seasonLength = 80;

        public bool opt_deckMode = true;
        public bool opt_windEnabled = true;
        public bool opt_huntEnabled = true;

        public override void onModsInitiallyLoaded()
        {
            kernel = this;
        }
        public override void beforeMapGen(Map map)
        {
            kernel = this;
        }
        public override void afterLoading(Map map)
        {
            kernel = this;
        }

        public override void onStartGamePresssed(Map map, List<God> gods)
        {
            gods.Add(new God_Season());
        }

        public override void receiveModConfigOpts_int(string optName, int value)
        {
            switch(optName)
            {
                case "Season Length":
                    kernel.opt_seasonLength = value;
                    break;
            }
        }

        public override void receiveModConfigOpts_bool(string optName, bool value)
        {
            switch(optName)
            {
                case "Deck of Seasons":
                    kernel.opt_deckMode = value;
                    break;
                case "Master of the Hunt Enabled":
                    kernel.opt_huntEnabled = value;
                    break;
                case "Painter of Winds Enabled":
                    kernel.opt_windEnabled = value;
                    break;
            }
        }


    }
}
