using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code;
using UnityEngine;

namespace CommunitySeasonGod
{

    //An aspect of the Season God with a unique power suite and other potential unique effects
    public abstract class SubGod
    {
        [SerializeField]
        protected God_Season _god;
        public God_Season God => _god;

        [SerializeField]
        protected Map _map;
        public Map Map => _map;

        //The SubGod-unique powers that get added when shifting to that season and removed when shifting out
        [SerializeField]
        private List<P_Season> _powers = new List<P_Season>();
        public List<P_Season> Powers => _powers;

        [SerializeField]
        protected List<int> _powerLevelReqs = new List<int>();
        public List<int> PowerLevelReqs => _powerLevelReqs;

        //SubGod-powers that get added when RANDOMLY shifting to that season and removed when shifting out - consider using P_Season_LimitedPower if you only want the player to cast them once or twice
        //Alternatively, non-power effects can be created in OnSeasonEntered
        [SerializeField]
        private List<P_Season> _bonusPowers = new List<P_Season>();
        public List<P_Season> BonusPowers => _bonusPowers;

        [SerializeField]
        private List<int> _bonusPowerLevelReqs = new List<int>();
        public List<int> BonusPowerLevelReqs => _bonusPowerLevelReqs;

        public SubGod(God_Season god, Map map)
        {
            _god = god;
            _map = map;
        }

        public virtual string GetName()
        {
            return "Placeholder Sub-God";
        }

        //Just two or three words. If not blank, Displays after the name in parentheses ( ) when using Seize the Throne, so players have some idea what they're getting into
        public virtual string GetKeywords()
        {
            return "(devastation, death)";
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

        #region Supplkicant
        public virtual Sprite GetSupplicantSprite()
        {
            return _map.world.textureStore.agent_supplicantSnake;
        }

        public virtual bool HasSupplicantStartingTraits()
        {
            return false;
        }

        public virtual List<Trait> GetSupplicantStartingTraits()
        {
            return new List<Trait>();
        }
        #endregion

        #region sub-god transition
        public virtual void OnActivate(Map map, SubGod previousSubGod, bool enteredNaturally)
        {

        }

        public virtual void OnDeactivate(Map map, SubGod nextSubGod, bool exitedNaturally)
        {

        }

        //Override this if you want your god to make something happen on god transitions that might not involve them at all
        public virtual void OnSubGodTransition(Map map, SubGod oldSubGod, SubGod newSubGod, bool transitionedNaturally)
        {

        }
        #endregion

        #region turnTick
        public virtual void TurnTick_Active(Map map)
        {

        }

        //Recommend not using unless you have an extremely good reason for player readability
        public virtual void TurnTick_Inactive(Map map)
        {

        }
        #endregion
    }
}
