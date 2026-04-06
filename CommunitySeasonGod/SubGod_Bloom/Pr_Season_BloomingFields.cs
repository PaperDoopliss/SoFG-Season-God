using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Pr_Season_BloomingFields : Property
    {

        public static int foodIncrease = 3;
        public static float habitabilityIncrease = 0.1f;
        public static double presencePerTurn = 0.5;

        public Pr_Season_BloomingFields(Location loc)
            : base(loc)
        {

        }

        public override string getName()
        {
            return "Blooming Fields";
        }

        public override string getDesc()
        {
            return "Plants grow easily and beautifully in this land. This location gains +" + foodIncrease + " <b>food</b> and +" + habitabilityIncrease * 100 + " <b>habitability</b> and increases local Fey Presence by " + presencePerTurn + "% per turn.";
        }

        public override Sprite getSprite(World world)
        {
            return EventManager.getImg("ComSeasonGod.power_blooming_fields.png");
        }

        public override bool removedOnRuin()
        {
            return false;
        }

        public override bool survivesRuin()
        {
            return true;
        }

        public override int getFoodGeneratedFlat()
        {
            return foodIncrease;
        }

        public override void turnTick()
        {
            base.turnTick();

            foreach (Property pr in location.properties)
            {
                if (pr is Pr_FeyPresence)
                {
                    pr.influences.Add(new ReasonMsg("Blooming Fields", presencePerTurn));
                    return;
                }
            }
            Pr_FeyPresence newPresence = new Pr_FeyPresence(location);
            newPresence.charge = presencePerTurn;
            location.properties.Add(newPresence);

        }

    }
}
