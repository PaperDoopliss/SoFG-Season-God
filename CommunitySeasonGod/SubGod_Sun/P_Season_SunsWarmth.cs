using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_SunsWarmth : P_Season
    {

        public static double temperatureFraction = 25;

        public P_Season_SunsWarmth(Map map) : base(map) { }

        public override string getName()
        {
            return "Sun's Warmth";
        }

        public override string getDesc()
        {
            return "Place Fey Presence at a location equal to " + temperatureFraction + "% of its temperature.";
        }

        public override string getFlavour()
        {
            return "The mad Patriarch resonates with the sun's heat, its warmth feeding and smothering life in equal measure.";
        }


        public override bool validTarget(Unit unit)
        {
            return false;
        }

        public override bool validTarget(Location loc)
        {
            return true;
        }

        public override int getCost()
        {
            return 1;
        }

        public override void cast(Location location)
        {
            base.cast(location);


            double amount = location.hex.getTemperature() * temperatureFraction;

            foreach (Property pr in location.properties)
            {
                if (pr is Pr_FeyPresence presence)
                {
                    presence.charge = Math.Min(300, presence.charge + amount);
                    return;
                }
            }

            Pr_FeyPresence newPresence = new Pr_FeyPresence(location);
            newPresence.charge = amount;
            location.properties.Add(newPresence);



        }
        
        public Sprite getSprite()
        {
            return map.world.iconStore.driveBackShadow;
        }

    }
}
