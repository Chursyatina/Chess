using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPFChessClient.Pages;
using WPFChessClient.Structures;
using static WPFChessClient.Pages.GamePlayPage;

namespace WPFChessClient.Logic
{
    class Presenter
    {
        public Figure[,] Board;

        private GamePlayPage Page;

        private List<Point> EditedCells;

        private Point SelectedFigurePosition;

        private FigureMoving figureMoving;

        private Player FirstPlayer;

        private Player SecondPlayer;

        private Player CurrentPlayer;

        private Player UnabledPlayer;

        private Move LastMove;

        public Presenter(GamePlayPage gameplayPage)
        {
            Page = gameplayPage;
            Board = FiguresStartPosition.GetFiguresStartPosition();
            EditedCells = new List<Point>();
            SelectedFigurePosition = new Point(-1,-1);
            figureMoving = new FigureMoving(Board);
            FirstPlayer = new Player(FiguresColor.white);
            SecondPlayer = new Player(FiguresColor.black);
            CurrentPlayer = FirstPlayer;
            UnabledPlayer = SecondPlayer;
            LastMove.SetStartPosition(new Point(0, 0));
            LastMove.SetEndPosition(new Point(0, 0));
            LastMove.SetFigure(Figures.Pawn);
        }

        public void ClickedOnBoard(Point coordCell)
        {
            Console.WriteLine(coordCell);
            if (SelectedFigurePosition != new Point(-1, -1))
            {
                if (CheckMovingPossibility(coordCell))
                {
                    Board = figureMoving.MakeMove(SelectedFigurePosition, coordCell, CurrentPlayer, UnabledPlayer);

                    ChangePlayer();
                    SelectedFigurePosition = new Point(-1, -1);
                }
                else
                {
                    SelectedFigurePosition = new Point(-1, -1);
                }
            }
            else
            {
                if (coordCell.Y >= 0 && coordCell.Y < 8 &&
                    coordCell.X >= 0 && coordCell.X < 8 &&
                    Board[(int)coordCell.Y, (int)coordCell.X] != null && 
                    Board[(int)coordCell.Y, (int)coordCell.X].Color == CurrentPlayer.GetFigureColor())
                {
                    SelectedFigurePosition = coordCell;
                }
            }
            CanvasUpdated();
        }

        public void CanvasUpdated()
        {
            Page.CalcBoardDimesions();

            Page.CreateBoard();

            EdmitPossibleMove();

            foreach (Point p in EditedCells)
            {
                Page.EdmitCell(p);
            }
            if (SelectedFigurePosition != new Point(-1, -1))
            {
                Page.EdmitCell(SelectedFigurePosition);
            }

            if (CurrentPlayer.GetCheckInfo())
            {
                for (int i = 0; i < BoardDimensions.CellCount; i++)
                {
                    for (int j = 0; j < BoardDimensions.CellCount; j++)
                    {
                        if (Board[i, j] != null &&
                            Board[i, j].GetType() == Figures.King &&
                            Board[i, j].Color == CurrentPlayer.GetFigureColor())
                            Page.EdmitCheck(new Point(j, i));
                    }
                }
            }

            for (int i = 0; i < BoardDimensions.CellCount; i++)
            {
                for (int j = 0; j < BoardDimensions.CellCount; j++)
                {
                    if (Board[i, j] != null)
                        Page.PutFigure(new Point(j, i), Board[i, j]);
                }
            }

            Page.UpdateCanvas();
        }

        private void EdmitPossibleMove()
        {
            if (SelectedFigurePosition == new Point(-1, -1))
            {
                EditedCells.Clear();
            }
            else
            {
                EditedCells = figureMoving.GetPossibleMoves(SelectedFigurePosition, CurrentPlayer, UnabledPlayer);
            }
        }

        private bool CheckMovingPossibility(Point moveCoordinate)
        {
            foreach (Point p in EditedCells)
            {
                if (p == moveCoordinate)
                {
                    return true;
                }
            }
            return false;
        }

        private void ChangePlayer()
        {
            if (CurrentPlayer == FirstPlayer)
            {
                CurrentPlayer = SecondPlayer;
                UnabledPlayer = FirstPlayer;
            }
            else
            {
                CurrentPlayer = FirstPlayer;
                UnabledPlayer = SecondPlayer;
            }
        }
    }
}
