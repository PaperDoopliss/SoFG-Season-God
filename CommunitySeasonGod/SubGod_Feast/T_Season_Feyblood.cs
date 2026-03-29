using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class T_Season_Feyblood : Trait
    {

        public static int spreadDuration = 20;
        public int spreadCooldown = 0;
        public Person source = null;
        public bool fromEmboldenBlood = false;
        public bool skipMessage = false;

        public Act_Season_GrandFeast act_feast;
        public Act_Season_Tournament act_tournament;
        public Rt_Season_GrandFeast rt_feast;

        public int buffMight = 1;
        public int buffLore = 1;
        public int buffIntrigue = 0;
        public int buffCommand = 1;

        public T_Season_Feyblood(Person recipient, Person source = null, bool fromEmboldenBlood = false) : base()
        {
            spreadCooldown = spreadDuration;
            this.source = source;
            this.fromEmboldenBlood = fromEmboldenBlood;

            act_feast = new Act_Season_GrandFeast(recipient.getLocation());
            act_tournament = new Act_Season_Tournament(recipient.getLocation());
            rt_feast = new Rt_Season_GrandFeast(recipient.getLocation());
        }

        public override string getName()
        {
            return "Feyblood (" + spreadCooldown + ")";
        }

        public override string getDesc()
        {
            if (buffMight == 1 && buffLore == 1 && buffIntrigue == 0 && buffCommand == 1)
                return "This person gets +1 to Might, Lore and Command, and gains liking for Ambition and Cruelty. If they are a ruler, they are less likely to join the Alliance, and gain ruler actions that drive them to plunder gold from other regions and spend that gold on extravagant ceremonies. While the Lord of the Feast is active, they will spread Feyblood to a direct descendant or vassal once every " + spreadDuration + " turns.";

            if (buffIntrigue > 0)
                return "This person gets various bonuses to all four stats and gains liking for Ambition and Cruelty. If they are a ruler, they are less likely to join the Alliance, and gain ruler actions that drive them to plunder gold from other regions and spend that gold on extravagant ceremonies. While the Lord of the Feast is active, they will spread Feyblood to a direct descendant or vassal once every " + spreadDuration + " turns.";

            return "This person gets various bonuses to Might, Lore and Command, and gains liking for Ambition and Cruelty. If they are a ruler, they are less likely to join the Alliance, and gain ruler actions that drive them to plunder gold from other regions and spend that gold on extravagant ceremonies. While the Lord of the Feast is active, they will spread Feyblood to a direct descendant or vassal once every " + spreadDuration + " turns.";

        }

        public override List<Assets.Code.Action> getActions()
        {
            List<Assets.Code.Action> result = base.getActions();

            result.Add(act_feast);
            result.Add(act_tournament);

            return result;
        }

        public override int getMightChange()
        {
            return buffMight;
        }

        public override int getLoreChange()
        {
            return buffLore;
        }

        public override int getIntrigueChange()
        {
            return buffIntrigue;
        }

        public override int getCommandChange()
        {
            return buffCommand;
        }

        public override int getMaxLevel()
        {
            return 3;
        }

        public override void onAcquire(Person person)
        {
            base.onAcquire(person);

            person.increasePreference(Tags.AMBITION);
            person.increasePreference(Tags.CRUEL);
            if (person.unit != null)
                person.unit.rituals.Add(rt_feast);

            if (skipMessage)
                return;

            if (source != null)
            {
                if (person.describeRelation(source) != "")
                    person.map.addUnifiedMessage(person, source, "Feyblood Spreads", "The taint of Feyblood has spread to " + person.getName() + " from their " + person.describeRelation(source) + " " + source.getName() + ". They gain liking for Ambition and Cruelty, and may perform unique actions if they rule a settlement.","FEYBLOOD SPREADS");
                else
                    person.map.addUnifiedMessage(person, source, "Feyblood Spreads", "The taint of Feyblood has spread to " + person.getName() + " from " + source.getName() + ". They gain liking for Ambition and Cruelty, and may perform unique actions if they rule a settlement.", "FEYBLOOD SPREADS");

            }
            else if (fromEmboldenBlood)
            {
                person.map.addUnifiedMessage(person, null, "Feyblood Spreads", person.getName() + "'s blood distorts as they consume the fey energies of this place. They have gained Feyblood, increasing their Might, Lore, and Command.", "FEYBLOOD SPREADS");
            }
            else
            {
                person.map.addUnifiedMessage(person, null, "Feyblood Spreads", person.getName() + " has been touched by the Pale Knight and infused with a hint of his wild, gluttonous nature. They gain liking for Ambition and Cruelty, and may perform unique actions if they rule a settlement.", "FEYBLOOD SPREADS");
            }
        }

        public override void turnTick(Person p)
        {
            base.turnTick(p);

            //Remove Great Feast if we're a controlled character and can't get anything out of it
            if (p.unit != null) {
                if (p.unit.isCommandable() == true)
                {
                    if (buffMight >= 3 && buffLore >= 3 && buffCommand >= 3 && buffIntrigue >= 3)
                    {
                        p.unit.rituals.Remove(rt_feast);
                    }
                }
                else if (p.unit.rituals.Contains(rt_feast) == false)
                    p.unit.rituals.Add(rt_feast);
            }
        
            //Armies under a Feyblood's control sack locations instead of capturing or raiding them
            if (p.rulerOf >= 0)
            {
                if (p.map.locations[p.rulerOf].settlement is SettlementHuman sh)
                {
                    if (sh.supportedMilitary != null)
                    {
                        if (sh.supportedMilitary.task is Task_RaidLocation || (sh.supportedMilitary.task is Task_CaptureLocation && p.society.isDarkEmpire == false))
                        {
                            sh.supportedMilitary.task = new Task_Season_SackLocation();
                        }
                    }
                }
            }


            if (p.map.overmind.god is God_Season season)
            {
                if (season.ActiveSubGod is SubGod_Feast)
                {
                    if (spreadCooldown > 1)
                        spreadCooldown--;
                    else
                    {
                        spreadCooldown = spreadDuration;
                        List<Person> possibleTargets = new List<Person>();


                        //A sovereign
                        if (p.society != null && p.society.getSovreign() == p)
                        {

                            foreach (Location l in p.society.lastTurnLocs)
                            {

                                if (l.settlement is SettlementHuman sh)
                                {

                                    //Also check for dwarven cities later on in the project
                                    if (sh is Set_City)
                                    {
                                        if (l.soc == p.society)
                                        {
                                            if (sh.ruler != null)
                                            {
                                                bool alreadyFeyblood = false;
                                                foreach (Trait t in sh.ruler.traits)
                                                {
                                                    if (t is T_Season_Feyblood)
                                                    {
                                                        alreadyFeyblood = true;
                                                        break;
                                                    }
                                                }

                                                if (!alreadyFeyblood)
                                                {
                                                    possibleTargets.Add(sh.ruler);
                                                }
                                            }
                                        }
                                    }
                                }

                            }

                            if (possibleTargets.Count > 0)
                            {
                                Person target = possibleTargets[Eleven.random.Next(possibleTargets.Count)];
                                target.receiveTrait(new T_Season_Feyblood(target, p));
                                return;
                            }

                            foreach (Location l in p.getLocation().getNeighbours())
                            {
                                if (l.soc == p.society)
                                {
                                    if (l.settlement is SettlementHuman sh)
                                    {
                                        if (sh.ruler != null)
                                        {
                                            bool alreadyFeyblood = false;
                                            foreach (Trait t in sh.ruler.traits)
                                            {
                                                if (t is T_Season_Feyblood)
                                                {
                                                    alreadyFeyblood = true;
                                                    break;
                                                }
                                            }

                                            if (!alreadyFeyblood)
                                            {
                                                possibleTargets.Add(sh.ruler);
                                            }
                                        }
                                    }
                                }
                            }
                            if (possibleTargets.Count > 0)
                            {
                                Person target = possibleTargets[Eleven.random.Next(possibleTargets.Count)];
                                target.receiveTrait(new T_Season_Feyblood(target, p));
                                return;
                            }

                        }

                        //A ruler
                        else if (p.rulerOf >= 0 && p.map.locations[p.rulerOf].settlement is SettlementHuman sh)
                        {

                            //A duke
                            //Add dwarven cities here at a later stage of the project
                            if (sh is Set_City)
                            {
                                foreach (Location l in p.getLocation().getNeighbours())
                                {
                                    if (l.soc == p.society)
                                    {
                                        if (l.settlement is SettlementHuman sh2)
                                        {
                                            if (sh2.ruler != null)
                                            {
                                                bool alreadyFeyblood = false;
                                                foreach (Trait t in sh2.ruler.traits)
                                                {
                                                    if (t is T_Season_Feyblood)
                                                    {
                                                        alreadyFeyblood = true;
                                                        break;
                                                    }
                                                }

                                                if (!alreadyFeyblood)
                                                {
                                                    possibleTargets.Add(sh2.ruler);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //A baron - spread it to any heroes or agents hanging out there
                            else
                            {

                                foreach (Assets.Code.Action a in sh.getLocalActions())
                                {
                                    if (a is Act_FundHero fund)
                                    {
                                        if (fund.hero() != null)
                                        {
                                            bool alreadyHasFeyblood = false;
                                            foreach (Trait t in fund.hero().traits)
                                            {
                                                if (t is T_Season_Feyblood)
                                                {
                                                    alreadyHasFeyblood = true;
                                                    break;
                                                }
                                            }

                                            if (!alreadyHasFeyblood)
                                                possibleTargets.Add(fund.hero());
                                        }
                                    }
                                }

                            }

                            if (possibleTargets.Count > 0)
                            {
                                Person target = possibleTargets[Eleven.random.Next(possibleTargets.Count)];
                                target.receiveTrait(new T_Season_Feyblood(target, p));
                                return;
                            }
                        }


                        //Direct descendants
                        foreach (Person p2 in p.map.persons)
                        {
                            if (p2.getParent() == p)
                            {

                                bool alreadyHasFeyblood = false;
                                foreach (Trait t in p2.traits)
                                {
                                    if (t is T_Season_Feyblood)
                                    {
                                        alreadyHasFeyblood = true;
                                        break;
                                    }
                                }

                                if (!alreadyHasFeyblood)
                                    possibleTargets.Add(p2);
                            }
                        }
                        if (possibleTargets.Count > 0)
                        {
                            Person target = possibleTargets[Eleven.random.Next(possibleTargets.Count)];
                            target.receiveTrait(new T_Season_Feyblood(target, p));
                            return;
                        }


                    }
                }
            }



        }

    }
}
