using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFChessClient.Interfaces;

namespace WPFChessClient.EventArgsClasses
{
    class ChangePageToGameArgs : IPageArgs
    {
        public NamePage Name { get; set; }

        public string FirstPlayerName;
        public string SecondPlayerName;

        public int GameTime;

        public ChangePageToGameArgs(NamePage name, string firstPlayerGame, string secondPlayerGame, int gameTime)
        {
            Name = name;
            FirstPlayerName = firstPlayerGame;
            SecondPlayerName = secondPlayerGame;
            GameTime = gameTime;
        }
    }
}
