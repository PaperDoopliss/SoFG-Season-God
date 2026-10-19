using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Assets.Code.Challenge;

namespace CommunitySeasonGod
{
    public class Rt_Season_RevelerRevelingTierIII : Ritual
    {
        public UA caster;

        public Rt_Season_RevelerRevelingTierIII(Location loc, UA parent)
            : base(loc)
        {
            caster = parent;
        }

        public override string getName()
        {
            return "Eternal Revelry";
        }

        public override string getCastFlavour()
        {
            return "";
        }

        public override string getDesc()
        {
            return "Grows an Eternal Revelry modifier while absorbing the profile and menace of any agent in the same location. Once the Eternal Revelry's modifier reaches 50%, its effects will activate. The Eternal Revelry will consume local Unrest and Lingering Resentment before dissipating.";
        }

        public override string getRestriction()
        {
            return "Requires a Fey Presence modifier with >= 100% charge and no existing Eternal Revelry modifier with >= 50% charge";
        }

        public override double getProfile()
        {
            return 10.0;
        }

        public override double getMenace()
        {
            return 5.0;
        }

        public override challengeStat getChallengeType()
        {
            return challengeStat.LORE;
        }

        public override double getProgressPerTurnInner(UA unit, List<ReasonMsg> msgs)
        {
            msgs?.Add(new ReasonMsg("Stat: Lore", Math.Max(1, unit.getStatLore())));
            return Math.Max(1, unit.getStatLore());
        }

        public override double getComplexity()
        {
            return 32.0;
        }

        public override int getCompletionMenace()
        {
            return 1;
        }

        public override int getCompletionProfile()
        {
            return 3;
        }

        public override bool validFor(UA ua)
        {
            return ua.location.settlement is SettlementHuman;
        }

        public override Sprite getSprite()
        {
            return map.world.iconStore.enshadow;
        }

        public override int isGoodTernary()
        {
            return -1;
        }

        public override bool isIndefinite()
        {
            return true;
        }

        public override void turnTick(UA ua)
        {
            base.turnTick(ua);
            ua.addProfile(3);
            ua.addMenace(3);
            Pr_Season_RevelerHarvest revelry = ua.location.properties.OfType<Pr_Season_RevelerHarvest>().FirstOrDefault();
            if (revelry != null)
            {
                if(revelry.charge < 50)
                {
                    revelry.influences.Add(new ReasonMsg("Reveling", caster.getStatLore()));
                }
                else
                {
                    revelry.hasActivated = true;
                    map.addUnifiedMessage(ua, ua.location, "Eternal Revelry Active", caster.getName() + " has established an Eternal Revelry in " + caster.location.getName() + ". While it is active, it will disrupt the local ruler and has a chance to disrupt any heroes who enter the location.\n\nIt will remain active until there is no more Unrest and Lingering Resentment in the location, or if the season changes.", "ETERNAL REVELRY");
                    ua.task = null;
                    return;
                }
            }
            else
            {
                revelry = new Pr_Season_RevelerHarvest(ua.location);
                revelry.charge = 2 + caster.getStatLore();
                ua.location.properties.Add(revelry);
            }

            if (ua.location.units.Count > 0)
            {
                foreach (Unit unit in ua.location.units)
                {
                    if (unit != null && unit != ua && unit is UA)
                    {
                        if (unit.isCommandable())
                        {
                            unit.addMenace(-ua.getStatLore());
                            unit.addProfile(-ua.getStatLore());
                        }

                        //if (unit is UAG hero && hero.person != null && hero.person.shadow < 1.0)
                        //{
                        //    unit.task = new Task_Disrupted(5);
                        //}
                    }
                }
            }
        }

        public override bool valid()
        {
            bool hasFeyPresence = false;
            bool hasEternalFestival = false;

            foreach (Property prop in location.properties)
            {
                if(prop != null)
                {
                    if(prop is Pr_FeyPresence presence)
                    {
                        if(presence.charge >= 100)
                        {
                            hasFeyPresence = true;
                        }
                    }

                    if(prop is Pr_Season_RevelerHarvest)
                    {
                        if(prop.charge >= 50)
                        {
                            hasEternalFestival = true;
                        }    
                    }
                }
            }

            if (hasEternalFestival)
            {
                return false;
            }

            if (hasFeyPresence)
            {
                return true;
            }

            return false;
        }

        public override int[] buildPositiveTags()
        {
            return new int[1] { Tags.DISCORD };
        }

        public override int[] buildNegativeTags()
        {
            return new int[0];
        }
    }
}
