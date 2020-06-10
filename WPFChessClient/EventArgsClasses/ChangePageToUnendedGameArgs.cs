using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using WPFChessClient.Interfaces;
using WPFChessClient.Logic;

namespace WPFChessClient.EventArgsClasses
{
    class ChangePageToUnendedGameArgs : IPageArgs
    {
        public NamePage Name { get; set ; }

        public string FirstPlayerName;
        public string SecondPlayerName;

        public Player FirstPlayer;
        public Player SecondPlayer;

        public Figure[,] Board;

        public Player CurrentPlayer;

        public ChangePageToUnendedGameArgs(Player first, Player second, string firstName, string secondName, Figure[,] board, Player currentPlayer)
        {
            FirstPlayerName = firstName;
            SecondPlayerName = secondName;

            FirstPlayer = first;
            SecondPlayer = second;

            Board = board;

            CurrentPlayer = currentPlayer;
        }

        public ChangePageToUnendedGameArgs()
        {

        }
    }
}
