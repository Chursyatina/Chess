using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFChessClient.Pages;
using static WPFChessClient.Pages.GamePlayPage;

namespace WPFChessClient.Logic
{
    class Bishop : Figure
    {
        public Bishop(Figures name, FiguresColor color) : base(name, color)
        {

        }
        public override List<Point> GetPossibleMoves(Point position)
        {
            List<Point> possibleMoves = new List<Point>();

            for (int i = 1; i < 8; i++)
            {
                possibleMoves.Add(new Point(position.X + i, position.Y + i));
                possibleMoves.Add(new Point(position.X + i, position.Y - i));
                possibleMoves.Add(new Point(position.X - i, position.Y - i));
                possibleMoves.Add(new Point(position.X - i, position.Y + i));

            }
            return possibleMoves;
        }

        public override Figures GetType()
        {
            return Figures.Bishop;
        }
    }
}
