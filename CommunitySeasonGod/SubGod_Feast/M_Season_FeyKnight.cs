using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class M_Season_FeyKnight : Minion
    {
        public M_Season_FeyKnight(Map map) : base(map)
        {

        }

        public override string getName()
        {
            return "Fey Knight";
        }

        public override Sprite getIcon()
        {
            return EventManager.getImg("ComSeasonGod.unit_pale_army.png");
        }

        public override Sprite getIconBack()
        {
            return map.world.textureStore.clear;
        }

        public override int getCommandCost()
        {
            return 2;
        }

        public override int getAttack()
        {
            return 5;
        }

        public override int getMaxDefence()
        {
            return 4;
        }

        public override int getMaxHP()
        {
            return 7;
        }

        public override int getGoldCost()
        {
            return map.param.minion_paladinCost;
        }

        public override Minion getClone()
        {
            return new M_Season_FeyKnight(map);
        }


    }
}
