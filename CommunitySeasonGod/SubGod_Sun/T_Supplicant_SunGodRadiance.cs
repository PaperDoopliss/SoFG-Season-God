using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    class T_Supplicant_SunGodsRadiance : T_SolEntity_SunCloak
    {

        public T_Supplicant_SunGodsRadiance() : base()
        {
            isInherent = true;
        }

        public override string getName()
        {
            return "Sun God's Radiance";
        }

        public override string getDesc()
        {
            return "This person gains " + getHPChange() + " <b>hp</b>. When another character attacks them in combat, they take " + reflection + " reflected damage, ignoring defence.";
        }

        public virtual int getHPChange()
        {
            return 2;
        }

    }
}