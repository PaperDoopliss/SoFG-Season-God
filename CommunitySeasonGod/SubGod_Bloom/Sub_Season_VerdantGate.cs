using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class Sub_Season_VerdantGate : Subsettlement
    {
        public static double madnessPerTurn = 10;
        public static double kudzuPerTurn = 10;
        public static double kudzuInNeighbours = 1;
        public static double vineTerrorSpawnCooldown = 10;

        public Sub_Season_VerdantGate(Settlement set)
            : base(set)
        {

            infiltrated = true;
        }

        public override string getName()
        {
            return "Verdant Gate";
        }

        public override string getHoverOverText()
        {
            return "A portal to the Niece of Blooming Fields' vast gardens. This location is locked to 100% <b>shadow</b>, gains " + madnessPerTurn + "% Madness and " + kudzuPerTurn + "% Dreaming Kudzu per turn, adds " + kudzuInNeighbours + "% Dreaming Kudzu to each neighbouring land location each turn, and summons a Vine Terror every " + vineTerrorSpawnCooldown + " turns.";
        }

        public override void turnTick()
        {
            base.turnTick();

            settlement.shadow = 1;
            menace += 1;

            bool foundMadness = false;
            bool foundKudzu = false;
            bool foundPurged = false;
            foreach (Property pr in settlement.location.properties)
            {
                if (pr is Pr_Madness)
                {
                    foundMadness = true;
                    pr.influences.Add(new ReasonMsg("Verdant Gate", madnessPerTurn));
                }
                else if (pr is Pr_Season_DreamingKudzu)
                {
                    foundKudzu = true;
                    pr.influences.Add(new ReasonMsg("Verdant Gate", kudzuPerTurn));
                }
                else if (pr is Pr_Season_PurgedKudzu)
                    foundPurged = true;
            }

            if (!foundMadness)
            {
                Pr_Madness madness = new Pr_Madness(settlement.location);
                madness.charge = madnessPerTurn;
                settlement.location.properties.Add(madness);
            }
            if (!foundKudzu && !foundPurged)
            {
                Pr_Season_DreamingKudzu kudzu = new Pr_Season_DreamingKudzu(settlement.location);
                kudzu.charge = kudzuPerTurn;
                settlement.location.properties.Add(kudzu);
            }

            foreach (Location l in settlement.location.getNeighbours())
            {
                bool foundKudzuInNeighbour = false;
                bool foundPurgedInNeighbour = false;

                foreach (Property pr in l.properties)
                {
                    if (pr is Pr_Season_DreamingKudzu)
                    {
                        foundKudzuInNeighbour = true;
                        pr.influences.Add(new ReasonMsg("Verdant Gate", kudzuInNeighbours));
                    }
                    else if (pr is Pr_Season_PurgedKudzu)
                        foundKudzuInNeighbour = true;
                }

                if (!foundKudzuInNeighbour && !foundPurgedInNeighbour)
                {
                    Pr_Season_DreamingKudzu kudzu = new Pr_Season_DreamingKudzu(l);
                    kudzu.charge = kudzuInNeighbours;
                    l.properties.Add(kudzu);
                }
            }

            if (settlement.map.turn % vineTerrorSpawnCooldown == 0)
            {
                Person p = new Person(settlement.map.soc_dark);
                UAEN_Season_VineTerror terror = new UAEN_Season_VineTerror(settlement.location, settlement.map.soc_dark, p);
                settlement.map.units.Add(terror);
                settlement.location.units.Add(terror);
            }

        }

        public override bool definesName()
        {
            return false;
        }

        public override Sprite getIcon()
        {
            return EventManager.getImg("ComSeasonGod.power_verdant_gate.png");

        }




    }
}
