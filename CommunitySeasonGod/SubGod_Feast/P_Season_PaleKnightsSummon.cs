using Assets.Code;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class P_Season_PaleKnightsSummon : P_Season
    {
        public P_Season_PaleKnightsSummon(Map map) : base(map) { }

        public int cost = 1;
        public static int costIncrement = 2;

        public override string getName()
        {
            return "Pale Knight's Summon";
        }

        public override string getDesc()
        {
            return "Inflicts the Pale Knight's Touch on a hero or ruler. This will eventually cause them to gain Feyblood, which increases their Might, Lore, and Command, makes them more aggressive, and grants them unique quests and actions to plunder and spend gold. Feyblood characters will spread Feyblood to a direct descendant or vassal every " + T_Season_Feyblood.spreadDuration + " turns. \n\nThis power's cost increases by " + costIncrement + " after every use.";
        }

        public override string getFlavour()
        {
            return "A figure in ornate golden armor drifts through unoccupied lands, leaving no footprints behind. His victim unerringly knows the being's location, and cannot escape the intrusive desire to meet him.";
        }

        public override string getRestrictionText()
        {
            return "Must target a hero or ruler without Feyblood or the Pale Knight's Touch";
        }

        public override Sprite getIconFore()
        {
            return EventManager.getImg("ComSeasonGod.power_pale_knights_summon.png");
        }

        public override bool validTarget(Location loc)
        {
            if (loc.settlement is SettlementHuman sh)
            {
                if (sh.ruler != null)
                {
                    foreach (Trait t in sh.ruler.traits)
                    {
                        if (t is T_Season_Feyblood || t is T_Season_PaleKnightsTouch)
                            return false;
                    }
                    return true;
                }
            }

            return false;
        }

        public override bool validTarget(Unit unit)
        {
            if (unit is UAG)
            {
                if (unit.person != null)
                {
                    foreach (Trait t in unit.person.traits)
                    {
                        if (t is T_Season_Feyblood || t is T_Season_PaleKnightsTouch)
                            return false;
                    }
                    return true;
                }
            }

            return false;
        }

        public override int getCost()
        {
            return cost;
        }

        public override void cast(Location location)
        {
            base.cast(location);

            if (location.settlement is SettlementHuman sh)
            {
                if (sh.ruler != null)
                {
                    cost += costIncrement;
                    sh.ruler.receiveTrait(new T_Season_PaleKnightsTouch(sh.ruler));
                }
            }

        }

        public override void cast(Unit unit)
        {
            base.cast(unit);
            if (unit.person != null)
            {
                cost += costIncrement;
                unit.person.receiveTrait(new T_Season_PaleKnightsTouch(unit.person));
            }
        }

    }
        
}
