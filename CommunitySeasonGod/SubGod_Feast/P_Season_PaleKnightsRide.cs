using Assets.Code;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{

    public class P_Season_PaleKnightsRide : P_Season
    {

        public double amount = 50;

        public P_Season_PaleKnightsRide(Map map) : base(map) { }


        public override string getName()
        {
            return "Pale Knight's Ride";
        }

        public override string getDesc()
        {
            return "A target location gains " + amount + "% Fey Presence, to a maximum of 300%.";
        }

        public override string getFlavour()
        {
            return "Where the Pale Knight wanders, strange creatures can be glimpsed out of the corner of one's eye, and those who follow them are never seen again.";
        }

        public override string getRestrictionText()
        {
            return "Can target any location with less than 300% Fey Presence";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_pale_knights_ride.png");
        }

        public override bool validTarget(Location loc)
        {
            if (loc.settlement is SettlementHuman)
            {
                foreach (Property pr in loc.properties)
                {
                    if (pr is Pr_FeyPresence && pr.charge >= 300)
                        return false;
                }
            }

            return true;
        }

        public override int getCost()
        {
            return 1;
        }

        public override void cast(Location location)
        {
            base.cast(location);


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


    }

}
