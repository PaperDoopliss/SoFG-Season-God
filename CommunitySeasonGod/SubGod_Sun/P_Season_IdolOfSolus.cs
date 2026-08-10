using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_IdolOfSolus : P_Season
    {
        public P_Season_IdolOfSolus(Map map) : base(map) { }

        public override string getName()
        {
            return "Idol of Solus";
        }

        public override string getDesc()
        {
            return "Creates an Idol of Solus at a location with Ancient Ruins. The Idol is an item that grants +" + I_Season_IdolofSolus.defence + " <b>defence</b> and reduces attackers' <b>hp</b> by " + I_Season_IdolofSolus.reflection + " when they start combat.";
        }

        public override string getFlavour()
        {
            return "The sun has been worshipped in countless forms for countless eons. The Patriarch conjures an idol from one of its more vindictive gods to punish those who would wrong his servants.";
        }

        public override string getRestrictionText()
        {
            return "Must target a location with Ancient Ruins";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_idol_of_solus.png");
        }
        public override bool validTarget(Unit unit)
        {
            return false;
        }

        public override bool validTarget(Location loc)
        {
            
            if (loc.settlement != null)
            {
                foreach (Subsettlement sub in loc.settlement.subs) 
                {
                    if (sub is Sub_AncientRuins) 
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override int getCost()
        {
            return 2;
        }

        public override void cast(Location location)
        {
            base.cast(location);

            Pr_ItemCache idol = new Pr_ItemCache(location);
            location.properties.Add(idol);
            idol.items[0] = new I_Season_IdolofSolus(map);


        }
    }
}