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
    public class P_Season_SunsEmbrace : P_Season
    {

        public static double basePresence = 25;
        public static double shadowConversionRate = 25;
        public static float maxShadowToRemove = 0.5f;

        public P_Season_SunsEmbrace(Map map) : base(map) { }

        public override string getName()
        {
            return "Sun's Embrace";
        }

        public override string getDesc()
        {
            return "Removes up to " + maxShadowToRemove * 100 + " <b>shadow</b> at a location and increases its Fey Presence by " + basePresence + "%, plus an additional " + shadowConversionRate + "% of the shadow removed.";
        }


        public override string getRestrictionText()
        {
            return "Can target any location";
        }

        public override string getFlavour()
        {
            return "The Patriarch's rays burn away the darkness, paving the way for something new and brutal.";
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
            return 0/*1*/;
        }

        public override void cast(Location loc)
        {
            base.cast(loc);


            float shadowToRemove = (float)Math.Min(loc.getShadow(), maxShadowToRemove);
            double presenceIncrease = basePresence + (loc.getShadow() * shadowConversionRate);


            if (loc.settlement != null)
            {
                loc.settlement.shadow -= shadowToRemove;
            }
            else
            {
                foreach (Hex hex in loc.territory)
                {
                    hex.purity += shadowToRemove;
                    if (hex.purity > 1)
                        hex.purity = 1;
                }
            }

            foreach (Property pr in loc.properties)
            {
                if (pr is Pr_FeyPresence presence)
                {
                    presence.charge = Math.Min(300, Math.Round(presence.charge + presenceIncrease));
                    return;
                }
            }

            Pr_FeyPresence newPresence = new Pr_FeyPresence(loc);
            newPresence.charge = Math.Round(presenceIncrease);
            loc.properties.Add(newPresence);


        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_suns_light.png");
        }

    }
}