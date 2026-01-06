using Assets.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunitySeasonGod
{
    public class T_Season_ExitStrategy : Trait
    {
        public T_Season_ExitStrategy() { }

        public override string getName()
        {
            return "Exit Strategy";
        }

        public override string getDesc()
        {
            return "This person gets -1 <b>attack</b> and +4 <b>defence</b>";
        }

        public override int getAttackChange()
        {
            return -1;
        }

        public override int getDefenceChange()
        {
            return 4;
        }

    }
}
