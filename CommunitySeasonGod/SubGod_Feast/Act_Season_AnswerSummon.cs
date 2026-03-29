using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Act_Season_AnswerSummon : Assets.Code.Action
    {

        public int turnTouched;

        public Act_Season_AnswerSummon(Location loc) : base(loc)
        {
            turnTouched = map.turn;
        }

        public override string getName()
        {
            return "Answer Summon";
        }

        public override string getShortDesc()
        {
            return "Gains Feyblood, increasing their stats and making them more indulgent and aggressive.";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_pale_knights_summon.png");
        }

        public override int getTurnsRequired()
        {
            return 5;
        }

        public override bool valid(Person ruler, SettlementHuman settlementHuman)
        {
            return true;
        }

        public override double getUtility(SettlementHuman hum, Person ruler, List<ReasonMsg> reasons)
        {
            double utility = base.getUtility(hum, ruler, reasons);

            reasons?.Add(new ReasonMsg("Growing Call", (map.turn - turnTouched) * 10));
            utility += (map.turn - turnTouched) * 10;

            return utility;
        }

        public override void complete()
        {
            base.complete();

            if (location.settlement is SettlementHuman sh && sh.ruler != null)
            {
                if (sh.ruler.unit != null)
                {
                    for (int i = 0; i < sh.ruler.unit.rituals.Count; i++)
                    {
                        if (sh.ruler.unit.rituals[i] is Rt_Season_AnswerSummon)
                        {
                            sh.ruler.unit.rituals.RemoveAt(i);
                            break;
                        }
                    }
                }

                for (int i = 0; i < sh.ruler.traits.Count; i++)
                {
                    if (sh.ruler.traits[i] is T_Season_PaleKnightsTouch)
                        sh.ruler.traits.RemoveAt(i);
                }

                sh.ruler.receiveTrait(new T_Season_Feyblood(sh.ruler));

            }

        }

    }
}
