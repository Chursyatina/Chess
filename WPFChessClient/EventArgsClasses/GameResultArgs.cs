using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFChessClient.Logic;
using static WPFChessClient.Pages.GamePlayPage;

namespace WPFChessClient.EventArgsClasses
{
    class GameResultArgs
    {
        public MoveResult MoveResult;

        public Player Attacker;

        public GameResultArgs(MoveResult moveResult, Player attacker)
        {
            MoveResult = moveResult;
            Attacker = attacker;
        }
    }
}
