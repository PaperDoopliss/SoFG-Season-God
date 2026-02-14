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
            //registry.RegisterHook_appliesGraphicalHexUpdate(Kernel_Season.Instance, appliesGraphicalHexUpdate); / Enable this line when the Community Library version 2.10.00 goes live on the 28th Feb 2026
        }

        public bool appliesGraphicalHexUpdate(Map map)
        {
            return map.overmind.god is God_Season && map.world.selector is Sel_CastPower castSelector && castSelector.power is P_HostileShift hostileShift;
        }

        public int onCalculateAgentsUsed(List<Unit> playerControlledUnits, int recruitmentCapUsed)
        {
            if (playerControlledUnits.Any(u => u is UAE_Supplicant))
            {
                return recruitmentCapUsed  - 1;
            }

            return recruitmentCapUsed;
        }
    }
}
