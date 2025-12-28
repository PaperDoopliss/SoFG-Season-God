using Assets.Code;
using Assets.Code.Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static SortedDictionaryProvider;

namespace CommunitySeasonGod
{
    public class UM_WiltedHorde : UM
    {
        public Set_WiltedGrove parent;

        public UM_WiltedHorde(Location loc, SocialGroup sg, Set_WiltedGrove grove, int size)
          : base(loc, sg)
        {
            this.parent = grove;
            this.maxHp = size;
        }

        public override string getName()
        {
            return "Wilted Horde";
        }

        public override Sprite getPortraitForeground()
        {
            return this.map.world.textureStore.unit_ravenousDead;
        }

        public override string getLeaderNameText()
        {
            return this.person != null ? "Lead by " + this.person.getName() : "";
        }

        public override void turnTickInner(Map map)
        {
            base.turnTickInner(map);
            var settlement = (SettlementHuman)this.location.settlement;
            var decay = this.location.properties.FirstOrDefault(pr => pr.getInvariantName() == DecayConsts.Wilting);

            if (settlement != null)
            {
                if (decay == null)
                {
                    this.location.properties.Add(new Pr_Wilting(this.location).WithCharges(2));
                }
                else {
                    decay.charge += 2;
                }

                if (decay.charge >= 100) P_Season_AutumnsCaress.ConvertToWiltedGrove(this.location);
                
            }
        }

        public override void inBattle(BattleArmy battleArmy)
        {
            base.inBattle(battleArmy);
        }

        public override void turnTickAI()
        {
            base.turnTickAI();
            if (this.location.settlement is SettlementHuman settlement1 && settlement1.shadow < 0.75)
            {
                this.task = new Task_RazeLocation() { ignorePeace = true };
            }
            else {
                int num = -1;
                Location loc = (Location)null;
                foreach (Location location in this.map.locations)
                {
                    if (location.settlement is SettlementHuman settlement && settlement.shadow < 0.75 && (loc == null || this.map.getStepDist(location, this.location) < num || this.map.getStepDist(location, this.location) == num && Eleven.random.Next(2) == 0))
                    {
                        num = this.map.getStepDist(location, this.location);
                        loc = location;
                    }
                    if (loc == null || this.map.getStepDist(location, this.location) < num || this.map.getStepDist(location, this.location) == num)
                    {
                        num = this.map.getStepDist(location, this.location);
                        loc = location;
                    }
                }
                if (loc == null)
                    return;
                this.task = (Task)new Task_GoToLocation(loc);
            }
        }
    }
}
