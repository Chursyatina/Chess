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
    class Knight : Figure
    {
        public Knight(Figures name, FiguresColor color) : base(name, color)
        {

        }
        public override List<Point> GetPossibleMoves(Point position)
        {
            List<Point> possibleMoves = new List<Point>();

            possibleMoves.Add(new Point(position.X - 1, position.Y - 2));
            possibleMoves.Add(new Point(position.X + 1, position.Y - 2));

            possibleMoves.Add(new Point(position.X - 1, position.Y + 2));
            possibleMoves.Add(new Point(position.X + 1, position.Y + 2));


            possibleMoves.Add(new Point(position.X + 2, position.Y - 1));
            possibleMoves.Add(new Point(position.X + 2, position.Y + 1));

            possibleMoves.Add(new Point(position.X - 2, position.Y - 1));
            possibleMoves.Add(new Point(position.X - 2, position.Y + 1));

            return possibleMoves;
        }

        public override GamePlayPage.Figures GetType()
        {
            return Figures.Knight;
        }
    }
}
