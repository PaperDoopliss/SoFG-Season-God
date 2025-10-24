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

        private static Kernel_Season _kernel;
        public static Kernel_Season Instance => _kernel;

        private static CommunityLib.ModCore _comLibKernel;
        public static CommunityLib.ModCore ComLibKernel => _comLibKernel;

        public static bool opt_deckMode = true;

        public static int opt_seasonLength = 80;
        public static int opt_windEnabled = 1;
        public static int opt_huntEnabled = 1;

        public static int GetSubGodEnabledState(SubGod subGod)
        {
            switch(subGod)
            {
                case SubGod_Hunt _:
                    return opt_huntEnabled;
                case SubGod_Wind _:
                    return opt_windEnabled;
                default:
                    return 0;
            }
        }

        public override void onModsInitiallyLoaded()
        {
            _kernel = this;
        }
        public override void beforeMapGen(Map map)
        {
            _kernel = this;

            GetModKernels(map);
        }
        public override void afterLoading(Map map)
        {
            _kernel = this;

            GetModKernels(map);
        }

        private void GetModKernels(Map map)
        {
            foreach (ModKernel kernel in map.mods)
            {
                switch(kernel.GetType().Namespace)
                {
                    case "CommunityLib":
                        _comLibKernel = kernel as CommunityLib.ModCore;
                        ComLibKernel.RegisterHooks(new ComLibHooks(map));
                        break;
                }
            }
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
                    opt_seasonLength = value;
                    break;
                case "Master of the Hunt Enabled":
                    opt_huntEnabled = value;
                    break;
                case "Painter of Winds Enabled":
                    opt_windEnabled = value;
                    break;
            }
        }
        public override void receiveModConfigOpts_bool(string optName, bool value)
        {
            switch(optName)
            {
                case "Deck of Seasons":
                    opt_deckMode=value;
                    break;
            }
        }
    }
}
