using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using WPFChessClient.Logic;

namespace WPFChessClient.EventArgsClasses
{
    public class SaveGameArgs
    {
        public Player FirstPlayer { get; private set; }
        public Player SecondPlayer{ get; private set; }
        public Player CurrentPlayer { get; private set; }
        public DispatcherTimer Timer { get; private set; }
        public Figure[,] Board { get; private set; }

        public SaveGameArgs(Player firstPlayer, Player secondPlayer, Player currentPlayer, DispatcherTimer timer, Figure[,] board)
        {
            FirstPlayer = firstPlayer;
            SecondPlayer = secondPlayer;
            CurrentPlayer = currentPlayer;
            Timer = timer;
            Board = board;
        }
    }
}
