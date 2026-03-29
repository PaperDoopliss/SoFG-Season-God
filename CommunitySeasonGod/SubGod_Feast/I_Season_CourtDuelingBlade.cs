using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class I_Season_CourtDuelingBlade : Item
    {
        public static int statBuff = 2;
        public bool thereWasAMinion = false;

        public I_Season_CourtDuelingBlade(Map map) : base(map) { }

        public override string getName()
        {
            return "Court Dueling Blade";
        }

        public override string getShortDesc()
        {
            return "A vicious blade that craves powerful souls. Grants +" + statBuff + " Might, and causes attacks to deal double damage if they do not target a minion.";
        }

        public override int getMightBonus()
        {
            return statBuff;
        }

        public override void onCombatRound(BattleAgents battleAgents, UA me, UA them)
        {
            base.onCombatRound(battleAgents, me, them);

            if (them.minions[0] != null && them.minions[0].isDead == false)
                thereWasAMinion = true;
            else
                thereWasAMinion = false;
        }

        public override void launchAttack(BattleAgents battle, UA me, UA them, int dealt, int defBefore)
        {
            base.launchAttack(battle, me, them, dealt, defBefore);

            if (!thereWasAMinion)
            {
                int toInflict = Math.Min(me.getStatAttack(), them.hp + them.defence);
                int defenceDamage = Math.Min(toInflict, them.defence);
                int hpDamage = Math.Min(toInflict - defenceDamage, them.hp);
                them.defence -= defenceDamage;
                them.hp -= hpDamage;

                if (battle.getGraphical() != null)
                {
                    battle.getGraphical().addMsg("Double Strike: " + me.getName() + " deals " + toInflict + " damage to " + them.getName(), Color.red);
                }

                if (them.hp <= 0)
                {
                    them.isDead = true;
                    me.onBattleKill(battle, me, them, battle.att, battle.def);
                    battle.getGraphical().addMsg(me.getName() + " has killed " + them.getName(), Color.white);

                }
            }
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.item_court_dueling_blade.png");
        }

        public override int getLevel()
        {
            return Item.LEVEL_ARTEFACT;
        }

        public override int getMorality()
        {
            return Item.MORALITY_NEUTRAL;
        }

    }
}
