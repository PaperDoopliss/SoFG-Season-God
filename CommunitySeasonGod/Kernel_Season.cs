using Assets.Code;
using Assets.Code.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


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

        public override void onCheatEntered(string command)
        {
            if (command != "fey")
            {
                return;
            }

            if (GraphicalMap.selectedHex != null && GraphicalMap.selectedHex.location != null)
            {
                Pr_FeyPresence feyPresence = (Pr_FeyPresence)GraphicalMap.selectedHex.location.properties.FirstOrDefault(pr => pr is Pr_FeyPresence);
                if (feyPresence != null)
                {
                    feyPresence.charge += 75;
                    if (feyPresence.charge > 300.0)
                    {
                        feyPresence.charge = 300.0;
                    }
                    return;
                }
                feyPresence = new Pr_FeyPresence(GraphicalMap.selectedHex.location);
                feyPresence.charge = 75.0;
                GraphicalMap.selectedHex.location.properties.Add(feyPresence);
            }
        }

        public override void onGraphicalHexUpdated(GraphicalHex graphicalHex)
        {
            if (graphicalHex == null || !(graphicalHex.map.overmind.god is God_Season) || !(graphicalHex.map.world.selector is Sel_CastPower castSelector) || !(castSelector.power is P_HostileShift hostileShift))
            {
                return;
            }

            if (!castSelector.canTarget(graphicalHex.hex))
            {
                graphicalHex.modifierStrength?.gameObject.SetActive(false);
                return;
            }

            Pr_FeyPresence feyPresence = (Pr_FeyPresence)graphicalHex.hex.location.properties.FirstOrDefault(pr => pr is Pr_FeyPresence);
            if (feyPresence == null)
            {
                graphicalHex.modifierStrength?.gameObject.SetActive(false);
                return;
            }

            if (graphicalHex.modifierStrength == null)
            {
                graphicalHex.modifierStrength = graphicalHex.world.prefabStore.getModifierStrength(graphicalHex.hex.location.getName(true), Color.white);
                graphicalHex.modifierStrength.gameObject.transform.SetParent(graphicalHex.transform);
                graphicalHex.modifierStrength.gameObject.transform.localPosition = new Vector3(0f, 0f, -3.02f);
                graphicalHex.modifierStrength.gameObject.transform.localScale = new Vector3(0.015f, 0.015f, 1f);
            }
            else
            {
                graphicalHex.modifierStrength.gameObject.SetActive(true);
                graphicalHex.modifierStrength.gameObject.transform.localScale = new Vector3(0.015f, 0.015f, 1f);
            }

            graphicalHex.modifierStrength.words.text = hostileShift.GetCost(graphicalHex.hex.location).ToString();
        }
    }
}
