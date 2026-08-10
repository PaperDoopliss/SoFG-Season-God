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
    public class Pr_Season_AshenEarth : Property
    {

        public Pr_Season_AshenEarth(Location loc) : base(loc)
        {

        }

        public override string getName()
        {
            return "Ashen Earth";
        }

        public override Sprite getSprite(World world)
        {
            return EventManager.getImg("ComSeasonGod.property_ashen_earth.png");
        }

        public override string getDesc()
        {
            return "The Solar Patriarch's wildfire has left mystical ashes behind, which can be used to fuel other Solar Patriarch powers";
        }

    }
}

