using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class UM_Season_GolemArmy : UM
    {

        public UM_Season_GolemArmy(Location loc, SocialGroup sg, int hp) : base(loc, sg)
        {
            this.hp = hp;
            maxHp = hp;
        }

        public override string getName()
        {
            return "Golem Army of " + society.getName();
        }

        public override Sprite getPortraitForeground()
        {
            return EventManager.getImg("ComSeasonGod.unit_golem_army.png");
        }

        public override void turnTickInner(Map map)
        {
            base.turnTickInner(map);
            if (!(task is Task_RazeLocation) || !(base.location.settlement is SettlementHuman settlementHuman) || !(society is SG_Orc sG_Orc) || sG_Orc.capital == -1 || map.locations[sG_Orc.capital].soc != sG_Orc || !(map.locations[sG_Orc.capital].settlement is Set_OrcCamp))
            {
                return;
            }

            Pr_OrcPlunder pr_OrcPlunder = null;
            foreach (Property property in map.locations[sG_Orc.capital].properties)
            {
                if (property is Pr_OrcPlunder pr_OrcPlunder2)
                {
                    pr_OrcPlunder = pr_OrcPlunder2;
                    break;
                }
            }

            if (pr_OrcPlunder == null)
            {
                pr_OrcPlunder = new Pr_OrcPlunder(map.locations[sG_Orc.capital]);
                map.locations[sG_Orc.capital].properties.Add(pr_OrcPlunder);
            }

            int num = (int)(map.param.ch_orcRazeGoldGainArmy * settlementHuman.prosperity * (double)settlementHuman.population);
            if (num <= 0)
            {
                return;
            }

            pr_OrcPlunder.gold += num;
            map.addMessage(getName() + " plunders " + num + " gold", 0.2, positive: true, base.location.hex);
            if (map.burnInComplete)
            {
                if (num >= 10)
                {
                    map.addUnifiedMessage(this, base.location, "Plunder", getName() + " plunders " + num + " gold from " + base.location.getName() + " while razing the settlement, which will be gathered in the orc main fortress's plunder, which your agents can access", UnifiedMessage.messageType.ORC_PLUNDER, force: true);
                }

                map.hintSystem.popHint(HintSystem.hintType.ORC_PLUNDER);
            }
        }

        public override void turnTickAI()
        {
            base.turnTickAI();
            if ((double)hp < (double)maxHp * 0.3)
            {
                if (locIndex == homeLocation)
                {
                    task = new Task_Recruit();
                }
                else
                {
                    task = new Task_GoToLocation(map.locations[homeLocation]);
                }

                return;
            }

            if (base.location.soc != null && society.getRel(base.location.soc).state == DipRel.dipState.war)
            {
                task = new Task_RazeLocation();
                return;
            }

            int num = -1;
            UM uM = null;
            foreach (Unit unit in map.units)
            {
                if (!(unit is UM_LuredCrowd) && unit is UM uM2 && (!(uM2 is UM_Refugees) || map.getStepDist(base.location, uM2.location) <= 1) && (uM2.location.soc == society || map.getStepDist(base.location, uM2.location) <= 4) && (unit.location.soc != unit.society || unit.hp >= unit.maxHp / 3) && !fightingMutualEnemy(uM2))
                {
                    DipRel rel = society.getRel(unit.society);
                    if ((rel.state == DipRel.dipState.war || (rel.state == DipRel.dipState.hostile && unit.location.soc == society)) && (uM == null || num > map.getStepDist(base.location, unit.location) || (num == map.getStepDist(base.location, unit.location) && Eleven.random.Next(2) == 0)))
                    {
                        num = map.getStepDist(base.location, unit.location);
                        uM = uM2;
                    }
                }
            }

            if (uM != null)
            {
                task = new Task_AttackArmy(uM, this);
                return;
            }

            num = -1;
            Location location = null;
            foreach (Location location2 in map.locations)
            {
                if (location2.soc != null && society.getRel(location2.soc).state == DipRel.dipState.war && (location == null || map.getStepDist(location2, base.location) < num))
                {
                    num = map.getStepDist(location2, base.location);
                    location = location2;
                }
            }

            if (location != null)
            {
                task = new Task_GoToLocation(location);
            }
            else if (locIndex == homeLocation)
            {
                if (hp < maxHp)
                {
                    task = new Task_Recruit();
                }
            }
            else
            {
                task = new Task_GoToLocation(map.locations[homeLocation]);
            }
        }

        public override bool checkForDisband(Map map)
        {
            if (!map.socialGroups.Contains(society))
            {
                task = null;
                base.location.units.Remove(this);
                map.units.Remove(this);
                isDead = true;
                return true;
            }

            return false;
        }

        public override int[] getPositiveTags()
        {
            return new int[2]
            {
            Tags.ORC,
            society.index + 20000
            };
        }

        public override string getLeaderNameText()
        {
            if (base.person != null)
            {
                return "Lead by " + base.person.getName();
            }

            return "";
        }



    }
}
