using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Pr_Season_FeyCrops : Property
    {


        public static int foodIncrease = 5;
        public static double prosperityIncrease = 0.05;
        public static double presencePerTurn = 1;
        public Act_Season_BurnFeyCrops act_burn;

        public Pr_Season_FeyCrops(Location loc)
            : base(loc)
        {
            act_burn = new Act_Season_BurnFeyCrops(loc, this);
        }

        public override string getName()
        {
            return "Fey Crops";
        }

        public override string getDesc()
        {
            return "Supernaturally-useful plant life grant this location +" + foodIncrease + " <b>food</b> and +" + prosperityIncrease * 100 + " <b>prosperity</b>, increases local Fey Presence by " + presencePerTurn + "% per turn, and allows the Niece of Blooming Fields to target nearby locations with powers. Aware rulers who do not have a positive opinion toward The Dark may choose to burn the Fey Crops down, inflicting devastation.";
        }

        public override Sprite getSprite(World world)
        {
            return EventManager.getImg("ComSeasonGod.power_fey_crops.png");
        }

        public override bool removedOnRuin()
        {
            return false;
        }

        public override bool survivesRuin()
        {
            return true;
        }

        public override double getProsperityInfluence()
        {
            return prosperityIncrease;
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
                    pr.influences.Add(new ReasonMsg("Fey Crops", presencePerTurn));
                    return;
                }
            }
            Pr_FeyPresence newPresence = new Pr_FeyPresence(location);
            newPresence.charge = presencePerTurn;
            location.properties.Add(newPresence);

        }

        public override List<Assets.Code.Action> getActions()
        {
            List<Assets.Code.Action> actions = new List<Assets.Code.Action>();
            actions.Add(act_burn);
            return actions;
        }

    }
}
