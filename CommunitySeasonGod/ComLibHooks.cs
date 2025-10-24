using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class ComLibHooks : CommunityLib.Hooks
    {
        public ComLibHooks(Map map)
            : base(map)
        {

        }

        public override int onCalculateAgentsUsed(List<Unit> playerControlledUnits, int recruitmentCapUsed)
        {
            if (playerControlledUnits.Any(u => u is UAE_Supplicant))
            {
                return recruitmentCapUsed--;
            }

            return recruitmentCapUsed;
        }
    }
}
