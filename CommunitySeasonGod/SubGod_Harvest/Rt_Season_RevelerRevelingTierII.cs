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
    public class Rt_Season_RevelerRevelingTierII : Ritual
    {
        public UA caster;

        public Rt_Season_RevelerRevelingTierII(Location loc, UA parent)
            : base(loc)
        {
            caster = parent;
        }

        public override string getName()
        {
            return "Fey Revelry";
        }

        public override string getCastFlavour()
        {
            return "";
        }

        public override string getDesc()
        {
            return "Increases local Fey Presence while increasing Lingering Resentment and disrupting the local ruler. Any agent sharing a location with this reveler will lose menace each turn based on the reveler's Lore.";
            //return "Actively disrupts the ruler (if they are not part of the Dark Empire) and any unenshadowed heroes in this location. Any agent sharing a location with this reveler will lose menace each turn based on the reveler's Lore. While the reveler is doing this ritual, local Fey Presence will grow.";
        }

        public override string getRestriction()
        {
            return "Requires Fey Presence with < 100% charge";
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
            return 15.0;
        }

        public override int getCompletionMenace()
        {
            return 3;
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

            ua.addProfile(2);
            ua.addMenace(3);

            Pr_FeyPresence presence = ua.location.properties.OfType<Pr_FeyPresence>().FirstOrDefault();
            if (presence != null)
            {
                if (presence.charge >= 100)
                {
                    ua.task = null;
                    return;
                }
                else
                {
                    presence.influences.Add(new ReasonMsg("Reveling", 2.0));
                    Property.addToProperty("Fey Revelry", Property.standardProperties.UNREST, 2.0, caster.location);
                }
            }

            Location loc = ua.location;
            if (loc != null && loc.person() != null)
            {
                if (loc.settlement != null && loc.settlement is SettlementHuman human)
                {
                    if (location.soc != null && location.soc is Society society && !society.isDarkEmpire)
                    {
                        if (human.actionUnderway != null)
                        {
                            human.actionUnderway = null;
                            human.actionUnderway = new Act_Gridlock(location);
                        }
                    }
                }

                if (loc.units.Count > 0)
                {
                    foreach (Unit unit in loc.units)
                    {
                        if (unit != null && unit != ua && unit is UA)
                        {
                            if (unit.isCommandable())
                            {
                                unit.addMenace(-ua.getStatLore());
                            }

                            //if (unit is UAG hero && hero.person != null && hero.person.shadow < 1.0)
                            //{
                            //    unit.task = new Task_Disrupted(5);
                            //}
                        }
                    }
                }
            }
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
