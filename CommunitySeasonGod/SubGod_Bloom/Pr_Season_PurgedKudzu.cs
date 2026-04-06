using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Pr_Season_PurgedKudzu : Property
    {

        public static double cooldown = 10;

        public Pr_Season_PurgedKudzu(Location loc)
            : base(loc)
        {
            charge = cooldown;
        }

        public override string getName()
        {
            return "Purged Kudzu";
        }

        public override string getDesc()
        {
            return "The Dreaming Kudzu has been fully uprooted in this place, and cannot spread here again for a short time.";
        }

        public override Sprite getSprite(World world)
        {
            return EventManager.getImg("ComSeasonGod.challenge_return_to_earth.png");
        }

        public override bool removedOnRuin()
        {
            return false;
        }

        public override bool survivesRuin()
        {
            return true;
        }

        public override bool deleteOnZero()
        {
            return true;
        }

        public override void turnTick()
        {
            base.turnTick();

            influences.Add(new ReasonMsg("Steady Regrowth", -1));
           
        }

    }
}
