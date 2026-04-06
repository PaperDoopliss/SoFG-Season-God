using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommunitySeasonGod
{
    public class UM_Season_FeyKnights : UM
    {

        public static int startingHP = 500;
        public static double presenceTargetCutoff = 100;
        public Location target = null;

        public UM_Season_FeyKnights(Location loc) : base(loc, loc.map.soc_dark)
        {
            maxHp = startingHP;
            hp = startingHP;
            updateFactionMembership();

        }

        public void lookForTarget()
        {
            int distance = 10000;

            foreach (Location l in map.locations)
            {
                if (l.settlement is SettlementHuman && l.soc?.isDark() == false)
                {
                    foreach (Property pr in l.properties)
                    {
                        if (pr is Pr_FeyPresence && pr.charge >= presenceTargetCutoff)
                        {
                            int currentDistance = map.getStepDist(l, location);
                            if (currentDistance < distance)
                            {
                                distance = currentDistance;
                                target = l;
                            }
                            break;
                        }
                    }
                }
            }

        }

        public void updateFactionMembership()
        {
            foreach (SocialGroup sg in map.socialGroups)
            {
                if (sg is Society soc)
                {
                    if (soc.checkIsGone() == false && soc.isDarkEmpire)
                    {
                        society = soc;
                        return;
                    }
                }
            }

            society = map.soc_dark;
        }

        public override void turnTickInner(Map map)
        {
            base.turnTickInner(map);

            updateFactionMembership();

            if (task == null || task is Task_GoToLocation)
                everyTurnAI();

            if (hp < maxHp)
            {
                Property toRemove = null;

                foreach (Property pr in location.properties)
                {
                    if (pr is Pr_FeyPresence)
                    {
                        double presenceToRemove = Math.Min(pr.charge, maxHp - hp);
                        if (presenceToRemove > 0)
                        {
                            hp += (int)presenceToRemove;
                            pr.charge -= presenceToRemove;

                            if (pr.charge <= 0)
                                toRemove = pr;
                        }

                        break;
                    }
                }

                if (toRemove != null)
                {
                    location.properties.Remove(toRemove);
                }
            }

        }

        public void everyTurnAI()
        {
            if (location.settlement is SettlementHuman sh)
            {

                Society soc = location.soc as Society;

                if (soc == null || soc.isDarkEmpire == false)
                {

                    if (society is Society mySoc && mySoc.isDarkEmpire && soc != null && soc.getRel(mySoc).state == DipRel.dipState.war)
                    {
                        task = new Task_CaptureLocation();
                        return;
                    }
                    Task_RazeLocation raze = new Task_RazeLocation();
                    raze.ignorePeace = true;
                    task = raze;
                }
            }
        }

        public override void turnTickAI()
        {
            base.turnTickAI();
            everyTurnAI();

            if (task == null)
            {
                if (target != null)
                {
                    if (location == target)
                        target = null;
                    else
                    {

                        double presenceAtLocation = 0;
                        foreach (Property pr in target.properties)
                        {
                            if (pr is Pr_FeyPresence)
                                presenceAtLocation += pr.charge;
                        }

                        if (presenceAtLocation < presenceTargetCutoff)
                            target = null;
                    }
                }

                if (target == null)
                    lookForTarget();

                if (target != null)
                {
                    task = new Task_GoToLocation(target);
                }
            }

        }

        public override bool isWanderingArmy()
        {
            return true;
        }

        public override bool checkForDisband(Map map)
        {
            return false;
        }

        public override string getName()
        {
            return "Fey Knights";
        }

        public override Sprite getPortraitForeground()
        {
            return EventManager.getImg("ComSeasonGod.unit_pale_army.png");
        }

        public override bool isCommandable()
        {
            return false;
        }





    }
}
