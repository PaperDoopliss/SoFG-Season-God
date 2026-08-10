using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    class I_Season_IdolofSolus : Item
    {
        public I_Season_IdolofSolus(Map map)
            : base(map) { }
        
        public static int reflection = 2;
        public static int defence = 1;
        
        public override string getName()
        {
            return "Idol of Solus";
        }

        public override string getShortDesc()
        {
            return "An idol of a long-gone solar deity. The holder of this item gains " + defence + " <b>defence</b> and causes any character attacking them to take " + reflection + " reflected damage, ignoring defence";
        }

        public override int getLevel()
        {
            return Item.LEVEL_ARTEFACT;
        }

        public override int getDefenceBonus()
        {
            return defence;
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.item_idol_of_solus.png");
        }

    }
}
