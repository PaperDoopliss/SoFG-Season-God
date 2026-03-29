using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class T_Season_KnightlyChallenger : Trait
    {

        public double preCombatMenace = -10;
        public double preCombatMinMenace = -10;

        public T_Season_KnightlyChallenger() : base()
        {

        }

        public override string getName()
        {
            return "Knightly Challenger";
        }

        public override string getDesc()
        {
            return "The turn after attacking a hero or acolyte with equal or greater <b>might</b>, this person loses any menace gained from that fight.";
        }

        public override void turnTick(Person p)
        {
            base.turnTick(p);

            if (preCombatMenace != -10)
            {
                if (p.unit != null)
                {
                    if (p.unit.inner_menaceMin > preCombatMenace)
                        p.unit.inner_menaceMin = preCombatMenace;

                    p.unit.setMenace(preCombatMenace);
                }
                preCombatMenace = -10;
                preCombatMinMenace = -10;
            }
        }

        public override void combatStart(UA ua, BattleAgents battleAgents)
        {
            base.combatStart(ua, battleAgents);

            if (preCombatMenace <= -10)
            {
                if (battleAgents.att == ua)
                {
                    if (battleAgents.att.getStatMight() <= battleAgents.def.getStatMight())
                    {
                        preCombatMenace = ua.menace;
                        preCombatMinMenace = ua.inner_menaceMin;
                    }
                }
                else if (battleAgents.def == ua)
                {
                    if (battleAgents.def.getStatMight() <= battleAgents.att.getStatMight())
                    {
                        preCombatMenace = ua.menace;
                        preCombatMinMenace = ua.inner_menaceMin;
                    }
                }
            }
        }

        public override int[] getTags()
        {
            return new int[1] { Tags.COMBAT };
        }
    }
}
