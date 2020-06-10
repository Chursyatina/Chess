using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFChessClient.EventArgsClasses;
using static WPFChessClient.Pages.GamePlayPage;

namespace WPFChessClient.Logic
{
    class FigureMoving
    {
        Figure[,] LogicBoard;

        private Move LastMove;

        private List<Figure> BlackEatenFigures;

        private List<Figure> WhiteEatenFigures;

        public event EventHandler<GameResultArgs> MoveDone;
        // -------------------------------------------------------------------------------------------------------------------------Конструктор
        public FigureMoving(Figure[,] logicBoard)
        {
            LogicBoard = logicBoard;
            BlackEatenFigures = new List<Figure>();
            WhiteEatenFigures = new List<Figure>();
        }
        //---------------------------------------------------------------------------------------------------------------------------Получение возможных ходов
        public List<Point> GetPossibleMoves(Point position, Player currentPlayer, Player unabledPlayer, Figure[,] board)
        {
            List<Point> validMoves;
            validMoves = board[(int)position.Y, (int)position.X].GetPossibleMoves(position);
            validMoves = FindValidMoves(position, validMoves, currentPlayer, board);
            validMoves = CheckShahExists(validMoves, position, currentPlayer, unabledPlayer, board);

            return validMoves;
        }

        private List<Point> FindValidMoves(Point position, List<Point> possibleMoves, Player currentPlayer, Figure[,] board)
        {
            List<Point> validMoves;
            validMoves = DeleteAbroadMoves(possibleMoves);

            validMoves = DeleteFrienlyPositionMoves(validMoves, position, board);

            validMoves = EditPawnsMoves(validMoves, position, board);

            validMoves = DeleteFencedOffMovesSecondVariant(validMoves, position, board);

            validMoves = CheckCastle(validMoves, position, currentPlayer, board);

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

        private List<Point> DeleteFrienlyPositionMoves(List<Point> possibleMoves, Point position, Figure[,] board)
        {
            List<Point> validMoves = new List<Point>();
            foreach (Point move in possibleMoves)
            {
                Figure selectedFigure = board[(int)position.Y, (int)position.X];
                Figure moveFigure = board[(int)move.Y, (int)move.X];

                if (moveFigure == null ||
                    moveFigure != null && moveFigure.Color != selectedFigure.Color)
                {
                    validMoves.Add(move);
                }
            }
            return validMoves;
        }

        private List<Point> DeleteFencedOffMovesSecondVariant(List<Point> possibleMoves, Point position, Figure[,] board)
        {
            if (board[(int)position.Y, (int)position.X].GetType() == Figures.Knight)
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
                    if (board[(int)currentMove.Y, (int)currentMove.X] != null && currentMove != move)
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

        private List<Point> EditPawnsMoves(List<Point> possibleMoves, Point position, Figure[,] board)
        {
            List<Point> validMoves = new List<Point>();
            if (board[(int)position.Y, (int)position.X].GetType() == Figures.Pawn)
            {
                foreach (Point move in possibleMoves)
                {
                    if (Math.Abs(move.Y - position.Y) == 2 && board[(int)move.Y, (int)move.X] == null)
                    {
                        if (CheckBrokenFieldMove(position, board))
                        {
                            validMoves.Add(move);
                        }
                        continue;
                    }
                    if (Math.Abs(move.X - position.X) == 1)
                    {
                        if (CheckPawnsTakeFigure(position, move, board))
                        {
                            validMoves.Add(move);
                        }
                        if (CheckEnPassant(position, move, board))
                        {
                            validMoves.Add(move);
                        }
                        continue;
                    }
                    if (board[(int)move.Y, (int)move.X] != null)
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

        private bool CheckBrokenFieldMove(Point position, Figure[,] board)
        {
            Figure[,] startPosition = FiguresStartPosition.GetFiguresStartPosition();
            if (startPosition[(int)position.Y, (int)position.X] != null &&
                startPosition[(int)position.Y, (int)position.X].Color == board[(int)position.Y, (int)position.X].Color &&
                board[(int)position.Y, (int)position.X].GetType() == startPosition[(int)position.Y, (int)position.X].GetType())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckPawnsTakeFigure(Point position, Point move, Figure[,] board)
        {
            if (board[(int)move.Y, (int)move.X] != null &&
                board[(int)position.Y, (int)position.X].Color != board[(int)move.Y, (int)move.X].Color)
            {
                return true;
            }
            return false;
        }

        private bool CheckEnPassant(Point position, Point move, Figure[,] board)
        {
            if (board[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X] != null &&
                board[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X].Color != board[(int)position.Y, (int)position.X].Color &&
                board[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X].GetType() == Figures.Pawn &&
                move.Y == ((LastMove.EndPosition.Y > LastMove.StartPosition.Y) ? LastMove.StartPosition.Y + 1 : LastMove.EndPosition.Y + 1) &&
                move.X == LastMove.EndPosition.X)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private List<Point> CheckCastle(List<Point> possibleMoves, Point position, Player currentPlayer, Figure[,] board)
        {
            List<Point> validMoves = new List<Point>();
            foreach (Point move in possibleMoves)
            {
                if (board[(int)position.Y, (int)position.X].GetType() == Figures.King)
                {
                    if (currentPlayer.GetLongCastleAbility() &&
                        Math.Abs(move.X - position.X) == 2 &&
                        board[(int)move.Y, (int)move.X - 1] == null &&
                        board[(int)move.Y, (int)move.X + 1] == null)
                    {
                        validMoves.Add(move);
                    }
                    if (currentPlayer.GetShortCastleAbility() &&
                       Math.Abs(move.X - position.X) == 2 &&
                       (board[(int)move.Y, (int)move.X - 1] == null || board[(int)move.Y, (int)move.X - 1].GetType() == Figures.Rook) &&
                       (board[(int)move.Y, (int)move.X + 1] == null || board[(int)move.Y, (int)move.X + 1].GetType() == Figures.Rook))
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

        private List<Point> CheckShahExists(List<Point> possibleMoves, Point position, Player checkPlayer, Player secondPlayer, Figure[,] board)
        {
            List<Point> validMoves = new List<Point>();
            foreach (Point move in possibleMoves)
            {
                Figure[,] possibleBoard = Copyer.GetCopy(board);
                possibleBoard[(int)move.Y, (int)move.X] = possibleBoard[(int)position.Y, (int)position.X];
                possibleBoard[(int)position.Y, (int)position.X] = null;
                if (!CheckShah(possibleBoard, checkPlayer, secondPlayer))
                {
                    checkPlayer.RemoveCheck();
                    validMoves.Add(move);
                }
            }
            return validMoves;
        }

        // ---------------------------------------------------------------------------------------------------------------------------Ход
        public Figure[,] MakeMove(Point startPosition, Point endPosition, Player currentPlayer, Player uncurrentPlayer)
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

            CheckShah(Copyer.GetCopy(LogicBoard), uncurrentPlayer, currentPlayer);

            currentPlayer.RemoveCheck();

            if (uncurrentPlayer.GetCheckInfo())
            {
                if (CheckForLackOfMoves(Copyer.GetCopy(LogicBoard), uncurrentPlayer, currentPlayer))
                {
                    if (uncurrentPlayer.GetCheckInfo())
                    {
                        GameResultArgs mate = new GameResultArgs(MoveResult.CheckMate, currentPlayer);
                        currentPlayer.GetWin();
                        MoveDone.Invoke(this, mate);
                        return LogicBoard;
                    }
                    else
                    {
                        GameResultArgs staleMate = new GameResultArgs(MoveResult.StaleMate, currentPlayer);
                        currentPlayer.GetWin();
                        MoveDone.Invoke(this, staleMate);
                        return LogicBoard;
                    }
                }
            }

            return LogicBoard;
        }

        private void ChangeFigurePosition(Point startPosition, Point endPosition)
        {
            if (LogicBoard[(int)startPosition.Y, (int)startPosition.X] != null)
            {
                LogicBoard[(int)endPosition.Y, (int)endPosition.X] = LogicBoard[(int)startPosition.Y, (int)startPosition.X];
                LogicBoard[(int)startPosition.Y, (int)startPosition.X] = null;
            }
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
                    new Queen(Figures.Queen, LogicBoard[(int)LastMove.StartPosition.Y, (int)LastMove.StartPosition.X].Color);
                LogicBoard[(int)LastMove.StartPosition.Y, (int)LastMove.StartPosition.X] = null;
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
                LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X + 1] = LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X - 2];
                LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X - 2] = null;
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
                LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X + 1] = LogicBoard[(int)LastMove.EndPosition.Y, (int)LastMove.EndPosition.X - 2];
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

        public bool CheckShah(Figure[,] board, Player checkPlayer, Player secondPlayer)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[j, i] != null &&
                        board[j, i].Color != checkPlayer.GetFigureColor())
                    {
                        List<Point> possibleFigureMoves = board[j, i].GetPossibleMoves(new Point(i, j));
                        possibleFigureMoves = FindValidMoves(new Point(i, j), possibleFigureMoves, secondPlayer, board);
                        foreach (Point move in possibleFigureMoves)
                        {
                            if (board[(int)move.Y, (int)move.X] != null &&
                                board[(int)move.Y, (int)move.X].GetType() == Figures.King &&
                                board[(int)move.Y, (int)move.X].Color == checkPlayer.GetFigureColor())
                            {
                                checkPlayer.GiveCheck();
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool CheckForLackOfMoves(Figure [,] board, Player checkPlayer, Player secondPlayer)
        {
            List<Point> allPossibleMoves = new List<Point>();
            List<Point> checkedFigurePossibleMoves = new List<Point>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[j, i] != null &&
                        board[j, i].Color == checkPlayer.GetFigureColor())
                    {
                        checkedFigurePossibleMoves = FindValidMoves(new Point(i, j),
                            board[j, i].GetPossibleMoves(new Point(i, j)),
                            checkPlayer,
                            board);
                        checkedFigurePossibleMoves = CheckShahExists(checkedFigurePossibleMoves, new Point(i, j), checkPlayer, secondPlayer, board);
                        allPossibleMoves.AddRange(checkedFigurePossibleMoves);
                    }
                }
            }
            if (allPossibleMoves.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // -----------------------------------------------------------------------------------------------------------------------------

    }
}
// ОБЯЗУЮ РАБОТАТЬ
