using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Rt_Season_SunGodWorship : Ritual
    {
        public static float temperatureToAdd = 0.05f;
        public static double presenceToAdd = 5;

        public Rt_Season_SunGodWorship(Location loc) : base(loc)
        {

        }

        public override string getName()
        {
            return "Sun-God Worship";
        }

        public override string getDesc()
        {
            return "Increases temperature at an Elven Settlement, a Farming Community, or a location on top of a forest hex, as well as its neighbours, by " + temperatureToAdd * 100 + "%";
        }

        public override string getCastFlavour()
        {
            return "";
        }

        public override string getRestriction()
        {
            return "";
        }

        public override double getProfile()
        {
            return 100;
        }

        public override double getMenace()
        {
            return 0;
        }

        public override Sprite getSprite()
        {
            return EventManager.getImg("ComSeasonGod.power_suns_light.png");
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.LORE;
        }

        public override bool validFor(UA ua)
        {
            if (ua.location.settlement is Set_ElvenCity)
            {
                return true;
            }

            if (ua.location.settlement != null)
            {
                foreach (Subsettlement sub in ua.location.settlement.subs)
                {
                    if (sub is Sub_Farms)
                        return true;
                }
            }

            if (location.hex.isForest)
                return true;

            return false;
        }

        public override bool valid()
        {
            return true;
        }

        public override int isGoodTernary()
        {
            return -1;
        }

        public override int getCompletionMenace()
        {
            return 5;
        }

        public override int getCompletionProfile()
        {
            return 3;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            int num = 0;
            num += unit.getStatLore();
            msgs?.Add(new ReasonMsg("Stat: Lore", unit.getStatLore()));

            if (num < 1)
            {
                num++;
                msgs?.Add(new ReasonMsg("Base", 1.0));
            }

            return num;
        }

        public override double getComplexity()
        {
            return 20;
        }

        public override void complete(UA u)
        {


            if (u is UAEN_SolEntity entity)
            {
                if (entity.completedLocations.Contains(u.location))
                    entity.completedLocations.Clear();
                entity.completedLocations.Add(u.location);
            }

            foreach (Hex hex in u.location.territory)
            {
                hex.transientTempDelta += temperatureToAdd;
            }
            foreach (Location l in u.location.getNeighbours())
            {
                foreach (Hex hex2 in l.territory)
                    hex2.transientTempDelta += temperatureToAdd;
            }

            if (u.location.settlement is Set_ElvenCity)
            {
                foreach (Property pr in u.location.properties)
                {
                    if (pr is Pr_FeyPresence presence)
                    {
                        presence.charge = Math.Min(300, presence.charge + presenceToAdd);
                        return;
                    }
                }

                Pr_FeyPresence newPresence = new Pr_FeyPresence(u.location);
                newPresence.charge = presenceToAdd;
                u.location.properties.Add(newPresence);
            }

        }

        public override double getUtility(UA ua, List<ReasonMsg> msgs)
        {
            double utility = base.getUtility(ua, msgs);
            return utility + 100;
        }

        public override int[] buildPositiveTags()
        {
            return new int[1]
            {
            Tags.CRUEL
            };
        }

        public override int[] buildNegativeTags()
        {
            return new int[1] { Tags.SHADOW };
        }



    }
}
