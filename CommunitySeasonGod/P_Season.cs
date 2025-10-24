using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{


    public class P_Season : Power
    {
        public P_Season(Map map) : base(map)
        {
        }

        public override void cast(Location loc)
        {
            base.cast(loc);
        }

        public override void cast(Unit unit)
        {
            base.cast(unit);
        }
    }
}
