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
            Board = FiguresStartPosition.GetFiguresInStartPosition();
            EditedCells = new List<Point>();
            SelectedFigurePosition = new Point(-1,-1);
            figureMoving = new FigureMoving();
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
                    LastMove.SetStartPosition(SelectedFigurePosition);
                    LastMove.SetEndPosition(coordCell);
                    LastMove.SetFigure(Board[(int)SelectedFigurePosition.Y, (int)SelectedFigurePosition.X].GetType());
                    //RemoveEatenFigure(coordCell);                    
                    FigureChangePosition(SelectedFigurePosition, coordCell);
                    figureMoving.CheckIsCheck(CurrentPlayer, UnabledPlayer, LastMove, Copyer.GetCopy(Board));
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

        private void FigureChangePosition(Point start, Point end)
        {
            RemoveEatenFigure(end);
            Figure figure = Board[(int)start.Y, (int)start.X];
            Board[(int)start.Y, (int)start.X] = null;
            Board[(int)end.Y, (int)end.X] = figure;
            FiguresColor color = figure.Color;
            if (CheckPawnTransformation(end))
            {
                Board[(int)end.Y, (int)end.X] = null;
                Board[(int)end.Y, (int)end.X] = new Queen(Figures.Queen, color);
            }
            CheckCastlesMoves();
            CheckCastlesAbilities();
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
                EditedCells = figureMoving.GetPossibleMoves(SelectedFigurePosition, Copyer.GetCopy(Board), LastMove, CurrentPlayer, UnabledPlayer);
                //EditedCells.Add(SelectedFigurePosition);
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

        private void RemoveEatenFigure(Point position)
        {
            Board[(int)position.Y, (int)position.X] = null;
            Point eatenPosition = figureMoving.GetEatenFigurePosition();
            if (eatenPosition.Y >= 0 && eatenPosition.Y < 8 && eatenPosition.X >= 0 && eatenPosition.Y < 8)
            {
                Board[(int)eatenPosition.Y, (int)eatenPosition.X] = null;
            }
        }

        private bool CheckPawnTransformation(Point position)
        {
            if (Board[(int)position.Y, (int)position.X].GetType() == Figures.Pawn &&
                (position.Y == 0 || position.Y == 7))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void CheckCastlesAbilities()
        {
            if (LastMove.Figure == Figures.King)
            {
                CurrentPlayer.DisableLongCastleAbility();
                CurrentPlayer.DisableShortCastleAbility();
            }
            if (LastMove.Figure == Figures.Rook && 
                (LastMove.StartPosition.X == 0 && LastMove.StartPosition.Y == 0) ||
                (LastMove.StartPosition.X == 0 && LastMove.StartPosition.Y == 7))
            {
                CurrentPlayer.DisableLongCastleAbility();
            }
            if (LastMove.Figure == Figures.Rook &&
                (LastMove.StartPosition.X == 7 && LastMove.StartPosition.Y == 0) ||
                (LastMove.StartPosition.X == 7 && LastMove.StartPosition.Y == 7))
            {
                CurrentPlayer.DisableShortCastleAbility();
            }
        }

        private void CheckCastlesMoves()
        {
            if (Math.Abs(LastMove.StartPosition.X - LastMove.EndPosition.X) == 2 &&
                LastMove.Figure == Figures.King &&
                Board[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X + 1] != null)
            {
                Board[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X - 1] = Board[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X + 1];
                Board[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X + 1] = null;
                return;
            }
            if (Math.Abs(LastMove.StartPosition.X - LastMove.EndPosition.X) == 2 &&
                LastMove.Figure == Figures.King &&
                LastMove.EndPosition.X <= 5 &&
                Board[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X + 2] != null)
            {
                Board[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X - 2] = Board[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X + 2];
                Board[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X + 2] = null;
                return;
            }
            if (Math.Abs(LastMove.StartPosition.X - LastMove.EndPosition.X) == 2 &&
                LastMove.Figure == Figures.King &&
                Board[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X - 1] != null)
            {
                Board[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X + 1] = Board[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X - 1];
                Board[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X - 1] = null;
                return;
            }
            if (Math.Abs(LastMove.StartPosition.X - LastMove.EndPosition.X) == 2 &&
                LastMove.Figure == Figures.King &&
                LastMove.EndPosition.X >= 2 &&
                Board[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X - 2] != null)
            {
                Board[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X + 2] = Board[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X - 2];
                Board[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X - 2] = null;
                return;
            }
        }
    }
}
