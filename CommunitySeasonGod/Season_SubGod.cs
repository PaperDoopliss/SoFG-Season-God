using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;

namespace CommunitySeasonGod
{

    //An aspect of the Season God with a unique power suite and other potential unique effects
    public class Season_SubGod
    {

        //The SubGod-unique powers that get added when shifting to that season and removed when shifting out
        public List<P_Season_SubGodPower> powers = new List<P_Season_SubGodPower>();
        public List<int> powerLevelReqs = new List<int>();
        
        //SubGod-powers that get added when RANDOMLY shifting to that season and removed when shifting out - consider using P_Season_LimitedPower if you only want the player to cast them once or twice
        //Alternatively, non-power effects can be created in OnSeasonEntered
        public List<P_Season_SubGodPower> bonusPowers = new List<P_Season_SubGodPower>();
        public List<int> bonusPowerLevelReqs = new List<int>();

        public virtual string GetName()
        {
            return "Placeholder Sub-God";
        }

        //Just two or three words. If not blank, Displays after the name in parentheses ( ) when using Seize the Throne, so players have some idea what they're getting into
        public virtual string GetKeywords()
        {
            return "";
        }

        //The god art for the top left corner. If empty, a placeholder art will display.
        public virtual string GetSpritePath() { return ""; }

        //The path for the event that runs when the season shifts to this god. If empty, no event will appear.
        public virtual string GetEventPath() { return ""; }

        //If populated, this event will run instead of the normal event if the Season Changes effect has happened. If empty, the event in GetEventPath will run in either case.
        public virtual string GetEventPathBonus() { return ""; }

        //If blank, a generic message displays if this SubGod is active at the Awakening seal break
        public virtual string GetAwakeningMessage() { return ""; }

        //If blank, a generic contextual message displays if this SubGod is active upon victory
        public virtual string GetVictoryMessage() { return ""; }

        public virtual void OnActivate(Map map, Season_SubGod previousSubGod, bool enteredNaturally)
        {

        }

        public virtual void OnDeactivate(Map map, Season_SubGod nextSubGod, bool exitedNaturally)
        {

        }

        public virtual void TurnTick_Active(Map map)
        {

        }

        //Recommend not using unless you have an extremely good reason for player readability
        public virtual void TurnTick_Inactive(Map map)
        {

        }

        //Override this if you want your god to make something happen on god transitions that might not involve them at all
        public virtual void OnSubGodTransition(Map map, Season_SubGod oldSubGod, Season_SubGod newSubGod, bool transitionedNaturally)
        {
            if (oldSubGod == this)
            {
                OnDeactivate(map, newSubGod, transitionedNaturally);
            }
            if (newSubGod == this)
            {
                OnActivate(map, oldSubGod, transitionedNaturally);
            }
        }

        //If you're implementing OnActivate effects, undo them from the map here if the player changes their mind about picking this SubGod
        public virtual void UndoActivation(Map map, bool activatedNaturally)
        {

        }

    }
}
