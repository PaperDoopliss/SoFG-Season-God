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
    public class Rt_Season_RevelerRevelingTierI : Ritual
    {
        public UA caster;

        public Rt_Season_RevelerRevelingTierI(Location loc, UA parent)
            : base(loc)
        {
            caster = parent;
        }

        public override string getName()
        {
            return "Revelry";
        }

        public override string getCastFlavour()
        {
            return "";
        }

        public override string getDesc()
        {
            return "Heals the land of unrest and devastation by converting it into fey presence. Any agent sharing a location with this character will slowly lose profile.";
        }

        public override string getRestriction()
        {
            return "Can only be done in a location with < 100% Fey Presence";
        }

        public override double getProfile()
        {
            return 10.0;
        }

        public override double getMenace()
        {
            return 0.0;
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

        public override void turnTick(UA ua)
        {
            double amountToAdd = 0;

            ua.addProfile(2);
            ua.addMenace(2);

            foreach (Property property in ua.location.properties)
            {
                if (property is Pr_Ward)
                {
                    ua.task = null;
                    return;
                }

                if (property is Pr_FeyPresence fey)
                {
                    if (fey.charge >= 100)
                    {
                        ua.task = null;
                        return;
                    }
                }

                /*if (property is Pr_Plague plague)
                {
                    amountToAdd += 1;
                    plague.influences.Add(new ReasonMsg("Reveling", -2.0));
                }

                if (property is Pr_Death death)
                {
                    amountToAdd += 1;
                    death.influences.Add(new ReasonMsg("Reveling", -2.0));
                }*/

                if (property is Pr_Unrest unrest)
                {
                    amountToAdd += 1;
                    unrest.influences.Add(new ReasonMsg("Reveling", -5.0));
                }

                if (property is Pr_Devastation devastation)
                {
                    amountToAdd += 1;
                    devastation.influences.Add(new ReasonMsg("Reveling", -5.0));
                }


            }

            /*if (ua.location.settlement is SettlementHuman settlementHuman)
            {
                settlementHuman.shadow += map.param.ch_ghastShadowPerTurnPerLore * (double)ua.getStatLore() * Math.Max(0.0, 1.0 );
                ua.addMenace(map.param.ch_ghastMenacePerTurn);
                ua.addProfile(map.param.ch_ghastProfilePerTurn);
                if (settlementHuman.shadow > 1.0)
                {
                    settlementHuman.shadow = 1.0;
                }
            }*/

            if(amountToAdd == 0)
            {
                ua.task = null;
                return;
            }

            foreach (Unit u in location.units)
            {
                if (u != null && u != ua && u is UA)
                {
                    if (u.isCommandable())
                    {
                        u.addProfile(-ua.getStatLore());
                    }
                }
            }

            Pr_FeyPresence presence = ua.location.properties.OfType<Pr_FeyPresence>().FirstOrDefault();
            if (presence != null)
            {
                presence.influences.Add(new ReasonMsg("Reveling", 1.0 + amountToAdd));
                return;
            }
            else
            {
                Pr_FeyPresence pr_FeyPresence = new Pr_FeyPresence(ua.location);
                pr_FeyPresence.charge = 2.0 + amountToAdd;
                ua.location.properties.Add(pr_FeyPresence);
            }

        }

        public override bool isIndefinite()
        {
            return true;
        }

        public override void complete(UA cast)
        {
        }

        public override bool valid()
        {
            return true;
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
