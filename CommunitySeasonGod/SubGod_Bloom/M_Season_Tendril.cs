using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class M_Season_Tendril : Minion
    {
        public M_Season_Tendril(Map map) : base(map) { }

        public override string getName()
        {
            return "Tendril";
        }

        public override Sprite getIcon()
        {
            return EventManager.getImg("ComSeasonGod.minion_tendril.png");
        }

        public override Sprite getIconBack()
        {
            return map.world.textureStore.clear;
        }

        public override int getCommandCost()
        {
            return 0;
        }

        public override int getAttack()
        {
            return 1;
        }

        public override int getRecruitmentTime()
        {
            return 1;
        }

        public override int getMaxDefence()
        {
            return 0;
        }

        public override int getMaxHP()
        {
            return 1;
        }

        public override Minion getClone()
        {
            return new M_Season_Tendril(map);
        }



    }
}
