﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WPFChessClient.EventArgsClasses;
using WPFChessClient.Pages;
using WPFChessClient.Saving;
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

        private FigureMoving FigureMoving;

        private Saver Saver;

        private Player FirstPlayer;

        private Player SecondPlayer;

        private Player CurrentPlayer;

        private Player UnabledPlayer;

        private Move LastMove;

        private DispatcherTimer Timer;

        private int PlayerTime;

        private bool MatchEnded = false;

        private bool FirstMove = true;

        public event EventHandler<SaveGameArgs> SavePressed;

        public Presenter(GamePlayPage gameplayPage, int time)
        {
            Page = gameplayPage;
            Board = FiguresStartPosition.GetFiguresStartPosition();
            EditedCells = new List<Point>();
            SelectedFigurePosition = new Point(-1, -1);
            FigureMoving = new FigureMoving(Board);
            PlayerTime = time*60;
            FirstPlayer = new Player(FiguresColor.white, PlayerTime);
            SecondPlayer = new Player(FiguresColor.black, PlayerTime);
            FirstPlayer.TimeIsUp += FirstPlayer_TimeIsUp;
            SecondPlayer.TimeIsUp += SecondPlayer_TimeIsUp;   
            CurrentPlayer = FirstPlayer;
            UnabledPlayer = SecondPlayer;
            LastMove.SetStartPosition(new Point(0, 0));
            LastMove.SetEndPosition(new Point(0, 0));
            LastMove.SetFigure(Figures.Pawn);
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(10000000);
            Timer.Tick += Timer_Tick;
            FigureMoving.MoveDone += ReactMoveResult;
            Page.SurrenderPresed += ReactSurrender;
            Page.SetTextTimerFirst(TimeToString(CurrentPlayer.Time));
            Page.SetTextTimerSecond(TimeToString(CurrentPlayer.Time));
            SavePressed += SaveGame;
            Saver = new Saver(Page.FirstPlayerName,Page.SecondPlayerName,Page.Time);
        }

        public Presenter(GamePlayPage gameplayPage, Player firstPlayer, Player secondPlayer, Player currentPlayer, Figure[,] board)
        {
            Page = gameplayPage;
            Board = Copyer.GetCopy(board);
            EditedCells = new List<Point>();
            SelectedFigurePosition = new Point(-1, -1);
            FigureMoving = new FigureMoving(Board);
            //PlayerTime = time;
            FirstPlayer = new Player(FiguresColor.white, firstPlayer.Time);
            SecondPlayer = new Player(FiguresColor.black, secondPlayer.Time);
            FirstPlayer.TimeIsUp += FirstPlayer_TimeIsUp;
            SecondPlayer.TimeIsUp += SecondPlayer_TimeIsUp;
            if (currentPlayer == firstPlayer)
            {
                CurrentPlayer = SecondPlayer;
                UnabledPlayer = FirstPlayer;
            }
            else
            {
                CurrentPlayer = FirstPlayer;
                UnabledPlayer = SecondPlayer;
            }
            LastMove.SetStartPosition(new Point(0, 0));
            LastMove.SetEndPosition(new Point(0, 0));
            LastMove.SetFigure(Figures.Pawn);
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(10000000);
            Timer.Tick += Timer_Tick;
            FigureMoving.MoveDone += ReactMoveResult;
            Page.SurrenderPresed += ReactSurrender;
            Page.SetTextTimerFirst(TimeToString(CurrentPlayer.Time));
            Page.SetTextTimerSecond(TimeToString(CurrentPlayer.Time));
            SavePressed += SaveGame;
            Saver = new Saver(Page.FirstPlayerName, Page.SecondPlayerName, FirstPlayer.Time);
        }


        private void FirstPlayer_TimeIsUp(object sender, EventArgs e)
        {
            FirstPlayer.GetWin();
            MakeMatchEnded();
            Page.SetTimesUpResult(CurrentPlayer);
            Timer.Stop();
        }

        private void SecondPlayer_TimeIsUp(object sender, EventArgs e)
        {
            SecondPlayer.GetWin();
            MakeMatchEnded();
            Page.SetTimesUpResult(CurrentPlayer);
            Timer.Stop();

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            CurrentPlayer.Time -= 1;
            if (CurrentPlayer == FirstPlayer)
            {
                Page.SetTextTimerFirst(TimeToString(CurrentPlayer.Time));
            }
            else
            {
                Page.SetTextTimerSecond(TimeToString(CurrentPlayer.Time));
            }
        }

        private String TimeToString(int time)
        {
            return String.Format("{0:00}:{1:00}", time / 60, time % 60);
        }

        public void ClickedOnBoard(Point coordCell)
        {

            Console.WriteLine(coordCell);
            if (!MatchEnded)
            {
                if (SelectedFigurePosition != new Point(-1, -1))
                {
                    if (CheckMovingPossibility(coordCell))
                    {
                        Board = FigureMoving.MakeMove(SelectedFigurePosition, coordCell, CurrentPlayer, UnabledPlayer);

                        ChangePlayer();
                        if (FirstMove)
                        {
                            Timer.Start();
                            FirstMove = false;
                        }
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
                EditedCells = FigureMoving.GetPossibleMoves(SelectedFigurePosition, CurrentPlayer, UnabledPlayer, Copyer.GetCopy(Board));
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
            Page.ChangeActiveBackgroung();
            Page.ChangeSurrenderVisibility();
        }

        private void ReactMoveResult(object sender, GameResultArgs result)
        {
            Page.SetMoveResult(result.MoveResult, result.Attacker);
            Timer.Stop();
            MakeMatchEnded();
        }

        private void ReactSurrender(MoveResult moveResult, Player player)
        {
            Page.SetMoveResult(moveResult, player);
            Timer.Stop();
            MakeMatchEnded();
        }

        private void MakeMatchEnded()
        {
            MatchEnded = true;
            Saver.SaveEndedGame(FirstPlayer,SecondPlayer);
            Page.MakeExitVisible();
        }

        public void ReactOmGameSaving()
        {
            SavePressed.Invoke(this, new SaveGameArgs(FirstPlayer, SecondPlayer, CurrentPlayer, Timer, Board));
        }

        private void SaveGame(object sender, SaveGameArgs e)
        {
            Saver.SaveUnendedgame(e);
        }
    }
}
