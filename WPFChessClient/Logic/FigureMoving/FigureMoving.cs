using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static WPFChessClient.Pages.GamePlayPage;

namespace WPFChessClient.Logic
{
    class FigureMoving
    {
        Figure[,] LogicBoard;

        private Move LastMove;

        private Point EatenFigurePosition;

        private List<Figure> BlackEatenFigures;

        private List<Figure> WhiteEatenFigures;
        // -------------------------------------------------------------------------------------------------------------------------Конструктор
        public FigureMoving(Figure[,] logicBoard)
        {
            LogicBoard = logicBoard;
            BlackEatenFigures = new List<Figure>();
            WhiteEatenFigures = new List<Figure>();
            EatenFigurePosition = new Point(-1, -1);
        }
        //---------------------------------------------------------------------------------------------------------------------------Получение возможных ходов
        public List<Point> GetPossibleMoves(Point position, Player currentPlayer, Player unabledPlayer)
        {
            List<Point> validMoves;
            validMoves = LogicBoard[(int)position.Y, (int)position.X].GetPossibleMoves(position);
            validMoves = FindValidMoves(position, validMoves, currentPlayer);

            return validMoves;
        }

        private List<Point> FindValidMoves(Point position, List<Point> possibleMoves, Player currentPlayer)
        {
            List<Point> validMoves;
            validMoves = DeleteAbroadMoves(possibleMoves);

            validMoves = DeleteFrienlyPositionMoves(validMoves, position);

            validMoves = EditPawnsMoves(validMoves, position);

            validMoves = DeleteFencedOffMovesSecondVariant(validMoves, position);

            validMoves = CheckCastle(validMoves, position, currentPlayer);

            return validMoves;
        }

        private List<Point> DeleteAbroadMoves(List<Point> possibleMoves)
        {
            List<Point> clearedList = new List<Point>();
            foreach (Point pos in possibleMoves)
            {
                if (pos.X < 8 && pos.X >= 0 && pos.Y < 8 && pos.Y >= 0)
                {
                    clearedList.Add(pos);
                }
            }
            return clearedList;
        }

        private List<Point> DeleteFrienlyPositionMoves(List<Point> possibleMoves, Point position)
        {
            List<Point> validMoves = new List<Point>();
            foreach (Point move in possibleMoves)
            {
                Figure selectedFigure = LogicBoard[(int)position.Y, (int)position.X];
                Figure moveFigure = LogicBoard[(int)move.Y, (int)move.X];

                if (moveFigure == null ||
                    moveFigure != null && moveFigure.Color != selectedFigure.Color)
                {
                    validMoves.Add(move);
                }
            }
            return validMoves;
        }

        private List<Point> DeleteFencedOffMovesSecondVariant(List<Point> possibleMoves, Point position)
        {
            if (LogicBoard[(int)position.Y, (int)position.X].GetType() == Figures.Knight)
            {
                return possibleMoves;
            }
            List<Point> validMoves = new List<Point>();
            foreach (Point move in possibleMoves)
            {
                Point currentMove = move;
                bool foundFigure = false;

                while (position != currentMove)
                {
                    if (LogicBoard[(int)currentMove.Y, (int)currentMove.X] != null && currentMove != move)
                    {
                        foundFigure = true;
                    }

                    if (position.X < currentMove.X) currentMove.X--;
                    else if (position.X > currentMove.X) currentMove.X++;

                    if (position.Y < currentMove.Y) currentMove.Y--;
                    else if (position.Y > currentMove.Y) currentMove.Y++;
                }
                if (!foundFigure)
                {
                    validMoves.Add(move);
                }
            }
            return validMoves;
        }

        private List<Point> EditPawnsMoves(List<Point> possibleMoves, Point position)
        {
            List<Point> validMoves = new List<Point>();
            if (LogicBoard[(int)position.Y, (int)position.X].GetType() == Figures.Pawn)
            {
                foreach (Point move in possibleMoves)
                {
                    if (Math.Abs(move.Y - position.Y) == 2 && LogicBoard[(int)move.Y,(int)move.X] == null)
                    {
                        if (CheckBrokenFieldMove(position))
                        {
                            validMoves.Add(move);
                        }
                        continue;
                    }
                    if (Math.Abs(move.X - position.X) == 1)
                    {
                        if (CheckPawnsTakeFigure(position, move))
                        {
                            validMoves.Add(move);
                        }
                        if (CheckEnPassant(position, move))
                        {
                            validMoves.Add(move);
                        }
                        continue;
                    }
                    if (LogicBoard[(int)move.Y, (int)move.X] != null)
                    {
                        continue;
                    }
                    validMoves.Add(move);
                }
                return validMoves;
            }
            else
            {
                return possibleMoves;
            }
        }

        private bool CheckBrokenFieldMove(Point position)
        {
            Figure[,] startPosition = FiguresStartPosition.GetFiguresStartPosition();
            if (startPosition[(int)position.Y, (int)position.X] != null &&
                startPosition[(int)position.Y, (int)position.X].Color == LogicBoard[(int)position.Y, (int)position.X].Color &&
                LogicBoard[(int)position.Y, (int)position.X].GetType() == startPosition[(int)position.Y, (int)position.X].GetType())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckPawnsTakeFigure(Point position, Point move)
        {
            if (LogicBoard[(int)move.Y, (int)move.X] != null &&
                LogicBoard[(int)position.Y, (int)position.X].Color != LogicBoard[(int)move.Y, (int)move.X].Color)
            {
                return true;
            }
            return false;
        }

        private bool CheckEnPassant(Point position, Point move)
        {
            if (LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X] != null &&
                LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X].Color != LogicBoard[(int)position.Y, (int)position.X].Color &&
                LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X].GetType() == Figures.Pawn &&
                move.Y == ((LastMove.EndPosition.Y > LastMove.StartPosition.Y) ? LastMove.StartPosition.Y + 1 : LastMove.EndPosition.Y + 1) &&
                move.X == LastMove.EndPosition.X)
            {
                EatenFigurePosition = LastMove.EndPosition;
                return true;
            }
            else
            {
                return false;
            }
        }

        private List<Point> CheckCastle(List<Point> possibleMoves, Point position, Player currentPlayer)
        {
            List<Point> validMoves = new List<Point>();
            foreach (Point move in possibleMoves)
            {
                if (LogicBoard[(int)position.Y, (int)position.X].GetType() == Figures.King)
                {
                    if (currentPlayer.GetLongCastleAbility() &&
                        Math.Abs(move.X - position.X) == 2 &&
                        LogicBoard[(int)move.Y, (int)move.X - 1] == null &&
                        LogicBoard[(int)move.Y, (int)move.X + 1] == null)
                    {
                        validMoves.Add(move);
                    }
                    if (currentPlayer.GetShortCastleAbility() &&
                       Math.Abs(move.X - position.X) == 2 &&
                       (LogicBoard[(int)move.Y, (int)move.X - 1] == null || LogicBoard[(int)move.Y, (int)move.X - 1].GetType() == Figures.Rook) &&
                       (LogicBoard[(int)move.Y, (int)move.X + 1] == null || LogicBoard[(int)move.Y, (int)move.X + 1].GetType() == Figures.Rook))
                    {
                        validMoves.Add(move);
                    }
                    if (Math.Abs(move.X - position.X) != 2)
                    {
                        validMoves.Add(move);
                    }
                }
                else
                {
                    validMoves.Add(move);
                }
            }
            return validMoves;
        }

        // ---------------------------------------------------------------------------------------------------------------------------Ход
        public Figure[,] MakeMove(Point startPosition, Point endPosition, Player currentPlayer, Player unabledPlayer)
        {
            LastMove.SetStartPosition(startPosition);
            LastMove.SetEndPosition(endPosition);
            LastMove.SetFigure(LogicBoard[(int)startPosition.Y, (int)startPosition.X].GetType());

            RemoveEatenFigure(endPosition);

            CheckPawnTransformation();

            CheckCastleMove();

            CheckCastlesAbilities(currentPlayer);

            CheckEnPassantMove();

            ChangeFigurePosition(startPosition, endPosition);

            return LogicBoard;
        }

        private void ChangeFigurePosition(Point startPosition, Point endPosition)
        {
            LogicBoard[(int)endPosition.Y, (int)endPosition.X] = LogicBoard[(int)startPosition.Y, (int)startPosition.X];
            LogicBoard[(int)startPosition.Y, (int)startPosition.X] = null;
        }

        private void RemoveEatenFigure(Point removePosition)
        {
            if (LogicBoard[(int)removePosition.Y, (int)removePosition.X] != null &&
                LogicBoard[(int)removePosition.Y, (int)removePosition.X].Color == FiguresColor.black)
            {
                BlackEatenFigures.Add(LogicBoard[(int)removePosition.Y, (int)removePosition.X]);
                LogicBoard[(int)removePosition.Y, (int)removePosition.X] = null;
            }

            if (LogicBoard[(int)removePosition.Y, (int)removePosition.X] != null &&
                LogicBoard[(int)removePosition.Y, (int)removePosition.X].Color == FiguresColor.white)
            {
                WhiteEatenFigures.Add(LogicBoard[(int)removePosition.Y, (int)removePosition.X]);
                LogicBoard[(int)removePosition.Y, (int)removePosition.X] = null;
            }
        }

        private void CheckPawnTransformation()
        {
            if (LastMove.Figure == Figures.Pawn &&
                (LastMove.EndPosition.Y == 7 || LastMove.EndPosition.Y == 0))
            {
                LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X] =
                    new Queen(Figures.Queen, LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X].Color);
            }
        }

        private void CheckCastleMove()
        {
            if (Math.Abs(LastMove.StartPosition.X - LastMove.EndPosition.X) == 2 &&
                LastMove.Figure == Figures.King &&
                LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X + 1] != null)
            {
                LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X - 1] = LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X + 1];
                LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X + 1] = null;
                return;
            }
            if (Math.Abs(LastMove.StartPosition.X - LastMove.EndPosition.X) == 2 &&
                LastMove.Figure == Figures.King &&
                LastMove.EndPosition.X <= 5 &&
                LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X + 2] != null)
            {
                LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X - 2] = LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X + 2];
                LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X + 2] = null;
                return;
            }
            if (Math.Abs(LastMove.StartPosition.X - LastMove.EndPosition.X) == 2 &&
                LastMove.Figure == Figures.King &&
                LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X - 1] != null)
            {
                LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X + 1] = LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X - 1];
                LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X - 1] = null;
                return;
            }
            if (Math.Abs(LastMove.StartPosition.X - LastMove.EndPosition.X) == 2 &&
                LastMove.Figure == Figures.King &&
                LastMove.EndPosition.X >= 2 &&
                LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X - 2] != null)
            {
                LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X + 2] = LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X - 2];
                LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X - 2] = null;
                return;
            }
        }

        private void CheckCastlesAbilities(Player currentPlayer)
        {
            if (LastMove.Figure == Figures.King)
            {
                currentPlayer.DisableLongCastleAbility();
                currentPlayer.DisableShortCastleAbility();
            }
            if (LastMove.Figure == Figures.Rook &&
                (LastMove.StartPosition.X == 0 && LastMove.StartPosition.Y == 0) ||
                (LastMove.StartPosition.X == 0 && LastMove.StartPosition.Y == 7))
            {
                currentPlayer.DisableLongCastleAbility();
            }
            if (LastMove.Figure == Figures.Rook &&
                (LastMove.StartPosition.X == 7 && LastMove.StartPosition.Y == 0) ||
                (LastMove.StartPosition.X == 7 && LastMove.StartPosition.Y == 7))
            {
                currentPlayer.DisableShortCastleAbility();
            }
        }

        private void CheckEnPassantMove()
        {
            if (LastMove.Figure == Figures.Pawn &&
                Math.Abs(LastMove.EndPosition.X - LastMove.StartPosition.X) == 1 &&
                LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X] == null)
            {
                RemoveEatenFigure(new Point(LastMove.EndPosition.X, LastMove.StartPosition.Y));
            }
        }
        // -----------------------------------------------------------------------------------------------------------------------------

    }
}
