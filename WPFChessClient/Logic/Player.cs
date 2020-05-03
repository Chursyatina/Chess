using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using static WPFChessClient.Pages.GamePlayPage;

namespace WPFChessClient.Logic
{
    class Player
    {
        private FiguresColor MyFigureColor;

        private bool ShortCastleAbility;
        private bool LongCastleAbility;
        private bool isCheck;

        private int Time;

        public Player(FiguresColor color, int time)
        {
            MyFigureColor = color;
            ShortCastleAbility = true;
            LongCastleAbility = true;
            isCheck = false;
            Time = time;
        }

        public void ReduceTime()
        {
            Time -= 1;
        }

        public FiguresColor GetFigureColor()
        {
            return MyFigureColor;
        }

        public void DisableLongCastleAbility()
        {
            LongCastleAbility = false;
        }

        public void DisableShortCastleAbility()
        {
            ShortCastleAbility = false;
        }

        public bool GetShortCastleAbility()
        {
            return ShortCastleAbility;
        }

        public bool GetLongCastleAbility()
        {
            return LongCastleAbility;
        }

        public bool GetCheckInfo()
        {
            return isCheck;
        }

        public void GiveCheck()
        {
            isCheck = true;
        }
        public void RemoveCheck()
        {
            isCheck = false;
        }
    }
}
