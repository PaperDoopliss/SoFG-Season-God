using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class I_Season_ServantsSpectacles : Item
    {
        public static int statBuff = 2;
        public static double presencePerTurn = 3;
        public Rt_Season_SpreadFeyPresence rt_spread;
        public Rt_Season_PurgeFeyPresence rt_purge;

        public I_Season_ServantsSpectacles(Map map) : base(map) 
        {
            rt_spread = new Rt_Season_SpreadFeyPresence(map.locations[0]);
            rt_purge = new Rt_Season_PurgeFeyPresence(map.locations[0]);
        }

        public override string getName()
        {
            return "Servant's Spectacles";
        }

        public override string getShortDesc()
        {
            return "Enchanted spectacles permitting their holder to perceive Fey Presence and granting them +" + statBuff + " <b>lore</b>. Characters who have over 50% shadow, are not aware, or have Feyblood will increase Fey Presence at their location by " + presencePerTurn + "% per turn. Members of the Alliance with this monocle can instead perform a challenge to remove Fey Presence at a location, and characters who are enshadowed or have Feyblood gain a challenge that lets them spread Fey Presence deliberately.";
        }

        public override int getLoreBonus()
        {
            return statBuff;
        }

        public override List<Ritual> getRituals(UA ua)
        {
            List<Ritual> result = new List<Ritual>();

            result.Add(rt_spread);

            if (ua.isCommandable() == false)
                result.Add(rt_purge);

            return result;
        }

        public override void turnTick(Person owner)
        {
            base.turnTick(owner);

            if ((owner.unit != null && owner.unit.isCommandable()) || owner.awareness < 1)
            {
                if (owner.getLocation() != null) {
                    foreach (Property pr in owner.getLocation().properties)
                    {
                        if (pr is Pr_FeyPresence)
                        {
                            pr.influences.Add(new ReasonMsg("Servant's Spectacles", presencePerTurn));
                            return;
                        }
                    }

                    Pr_FeyPresence newPresence = new Pr_FeyPresence(owner.getLocation());
                    newPresence.charge = presencePerTurn;
                    owner.getLocation().properties.Add(newPresence);
                }

            }

        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.item_servants_spectacles.png");
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
