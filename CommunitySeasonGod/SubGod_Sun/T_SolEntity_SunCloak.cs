using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    class T_SolEntity_SunCloak : Trait
    {

        public static int reflection = 1;

        public int numberOfItems = 0;
        public bool isInherent = false;

        public override string getName()
        {
            return "Sun Cloak";
        }

        public override string getDesc()
        {
            return "This entity causes any character damaging them to suffer " + reflection + " reflected damage, ignoring <b>defence</b>";
        }

    }
}
