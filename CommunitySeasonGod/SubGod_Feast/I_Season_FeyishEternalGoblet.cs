using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class I_Season_FeyishEternalGoblet : Item
    {
        public static int statBuff = 2;
        public static double hpDivisor = 4;

        public I_Season_FeyishEternalGoblet(Map map) : base(map) { }

        public override string getName()
        {
            return "Feyish Eternal Goblet";
        }

        public override string getShortDesc()
        {
            return "An everflowing goblet of a supernaturally-invigorating wine. Grants +" + statBuff + " Intrigue, and heals " + (100f / hpDivisor) + "% HP every turn (rounding down, minimum 1).";
        }

        public override int getIntrigueBonus()
        {
            return statBuff;
        }

        public override void turnTick(Person owner)
        {
            base.turnTick(owner);

            if (owner.unit != null)
            {
                int toHeal = Convert.ToInt32(Math.Floor((double)(owner.unit.maxHp) / hpDivisor));
                if (toHeal < 1)
                    toHeal = 1;

                owner.unit.hp = Math.Min(owner.unit.maxHp, owner.unit.hp + toHeal);
            }

        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.item_feyish_eternal_goblet.png");
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
