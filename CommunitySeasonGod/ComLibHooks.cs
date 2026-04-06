using Assets.Code;
using CommunityLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class ComLibHooks
    {
        public ComLibHooks(CommunityLib.ModCore comLibKernel, Map map)
        {
            HooksDelegateRegistry registry = comLibKernel.HookRegistry;
            registry.RegisterHook_onCalculateAgentsUsed(onCalculateAgentsUsed);
            registry.RegisterHook_appliesGraphicalHexUpdate(Kernel_Season.Instance, appliesGraphicalHexUpdate);
        }

        public bool appliesGraphicalHexUpdate(Map map)
        {
            return map.overmind.god is God_Season && map.world.selector is Sel_CastPower castSelector && castSelector.power is P_HostileShift hostileShift;
        }

        public int onCalculateAgentsUsed(List<Unit> playerControlledUnits, int recruitmentCapUsed)
        {
            if (World.staticMap.overmind.god is God_Season)
            {
                int unitMod = 0;

                foreach (Unit u in playerControlledUnits)
                {
                    if (u is UAE_Supplicant || u is UAE_Season_GardenNymph || u is UAE_Season_PaleKnight)
                        unitMod--;
                }


                return recruitmentCapUsed + unitMod;
            }


            return recruitmentCapUsed;
        }
    }
}
