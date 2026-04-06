using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class T_Season_SuppressedTrait : Trait
    {

        public Trait oldTrait;

        public T_Season_SuppressedTrait(Trait oldTrait) : base()
        {
            this.oldTrait = oldTrait;
        }

        public override string getName()
        {
            return "X: " + oldTrait.getName();
        }

        public override string getDesc()
        {
            return "This person's " + oldTrait.getName() + " trait is suppressed until they once again fall under your control";
        }

        public static void replaceTraits(Person p)
        {
            if (p == null)
                return;

            foreach (Trait t in p.traits.ToList())
            {
                if (t is T_Howl_Madness || t is T_Howl_Sin || t is T_DarkAristocracy || t is T_TheyWillObey || t is T_Epidemiologist || t is T_Mortician || t is T_MonkeyPickpocket || t is T_Ophanim_Duality | t is T_Ophanim_Inquisitor || t is T_Ophanim_LeaderOfTheFaith || t is T_Ophanim_Preacher || t is T_Snake_Enshadower || t is T_TheSettingSun || t is T_Iastur_MaddeningTongues)
                {
                    p.traits.Remove(t);
                    p.traits.Add(new T_Season_SuppressedTrait(t));
                }
            }
        }


        public override void turnTick(Person p)
        {
            base.turnTick(p);

            if (p.unit != null)
            {
                if (p.unit.isCommandable())
                {
                    p.traits.Remove(this);
                    p.traits.Add(oldTrait);
                }
            }

        }


    }
}
