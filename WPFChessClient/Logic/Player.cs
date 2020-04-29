using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WPFChessClient.Pages.GamePlayPage;

namespace WPFChessClient.Logic
{
    class Player
    {
        FiguresColor MyFigureColor;

        bool ShortCastleAbility;
        bool LongCastleAbility;
        bool isCheck;

        public Player(FiguresColor color)
        {
            MyFigureColor = color;
            ShortCastleAbility = true;
            LongCastleAbility = true;
            isCheck = false;
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
