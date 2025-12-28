//using Assets.Code;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace CommunitySeasonGod
//{
//    public class Act_SpreadWilt : Action
//    {
//        public List<Challenge> challenges = new List<Challenge>();
//        public int Size { get; private set; }

//        public Act_SpreadWilt(Location loc)
//          : base(loc)
//        {
//            this.map = loc.map;
//            this.locationIndex = loc.index;
//        }

//        public override string getName() => "Spread Wilt";

//        public override Sprite getIconFore() => this.map.world.iconStore.mushroomFarms;

//        public override Sprite getIconBack() => this.map.world.iconStore.standardBack;

//        public override int getTurnsRequired() => 7;

//        public override string getShortDesc()
//        {
//            return "Raise an Army of Wilted.";
//        }

//        public override double getUtility(SettlementHuman hum, Person ruler, List<ReasonMsg> reasons)
//        {
//            return utility2;
//        }

//        public override int[] getPositiveTags()
//        {
//            return new int[1] { Tags.GOLD };
//        }

//        public override void complete()
//        {
//            SettlementHuman settlement;
//            int num;
//            if (this.location.person() != null)
//            {
//                settlement = this.location.settlement as SettlementHuman;
//                num = settlement != null ? 1 : 0;
//            }
//            else
//                num = 0;
//            if (num != 0)
//                this.location.person().gold += (int)(settlement.prosperity * (double)this.map.param.act_tax_citizens_goldPerProsperity * ((double)settlement.population / 100.0));
//            Property.addToPropertySingleShot("Taxes", Property.standardProperties.UNREST, (double)this.map.param.act_tax_citizens_resentment, this.location);
//        }

//    }
//}
