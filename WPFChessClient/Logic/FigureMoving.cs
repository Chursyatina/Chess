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

        private Point EatenFigurePosition;

        public FigureMoving()
        {
            LogicBoard = new Figure[8, 8];
            FillTheBoard();
            EatenFigurePosition = new Point(-1, -1);
        }

        public void FillTheBoard()
        {
            LogicBoard = FiguresStartPosition.GetFiguresInStartPosition();
        }

        public List<Point> GetPossibleMoves(Point position, Figure[,] board, Move lastMove, Player currentPlayer, Player unabledPlayer)
        {
            LogicBoard = board;
            List<Point> validMoves;
            validMoves = LogicBoard[(int)position.Y, (int)position.X].GetPossibleMoves(position);
            validMoves = FindValidMoves(position, LogicBoard, validMoves, lastMove, currentPlayer);
            
            if (currentPlayer.GetCheckInfo())
            {
                validMoves = FindCheckRemoveMove(validMoves,Copyer.GetCopy(LogicBoard), position, currentPlayer, unabledPlayer, lastMove);
            }

            return validMoves;
        }

        public List<Point> FindValidMoves(Point position, Figure[,] board, List<Point> possibleMoves, Move lastMove, Player currentPlayer)
        {
            List<Point> validMoves;
            validMoves = DeleteAbroadMoves(possibleMoves);
            validMoves = DeleteFrienlyPositionMoves(validMoves, position, board);
            validMoves = EditPawnsMoves(validMoves, position, board, lastMove);
            validMoves = DeleteFencedOffMovesSecondVariant(validMoves, position, board);
            if (board[(int)position.Y, (int)position.X].GetType() == Figures.King)
            {
                validMoves = CheckCastle(validMoves, position, board, currentPlayer);
            }
            return validMoves;
        }

        public List<Point> DeleteAbroadMoves(List<Point> possibleMoves)
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

        public List<Point> DeleteFrienlyPositionMoves(List<Point> possibleMoves, Point position, Figure[,] board)
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

        public List<Point> DeleteFencedOffMovesFirstVariant(List<Point> possibleMoves, Point position, Figure[,] board)
        {
            if (board[(int)position.Y, (int)position.X].GetType() == Figures.Knight)
            {
                return possibleMoves;
            }
            List<Point> validMoves = new List<Point>();
            foreach (Point move in possibleMoves)
            {
                if (position.X == move.X && position.Y > move.Y)
                {
                    for (int i = (int)move.Y + 1; i < position.Y; i++)
                    {
                        if (board[(int)i, (int)position.X] != null)
                        {
                            break;
                        }
                        if (i == position.Y - 1)
                        {
                            validMoves.Add(move);
                        }
                    }
                    if (position.Y - move.Y == 1)
                    {
                        validMoves.Add(move);
                    }
                }

                if (position.X == move.X && position.Y < move.Y)
                {
                    for (int i = (int)position.Y + 1; i < move.Y; i++)
                    {
                        if (board[(int)i, (int)position.X] != null)
                        {
                            break;
                        }
                        if (i == move.Y - 1)
                        {
                            validMoves.Add(move);
                        }
                    }
                    if (move.Y - position.Y == 1)
                    {
                        validMoves.Add(move);
                    }
                }

                if (position.X > move.X && position.Y == move.Y)
                {
                    for (int i = (int)move.X + 1; i < position.X; i++)
                    {
                        if (board[(int)position.Y, (int)i] != null)
                        {
                            break;
                        }
                        if (i == position.X - 1)
                        {
                            validMoves.Add(move);
                        }
                    }
                    if (position.X - move.X == 1)
                    {
                        validMoves.Add(move);
                    }
                }

                if (position.X < move.X && position.Y == move.Y)
                {
                    for (int i = (int)position.X + 1; i < move.X; i++)
                    {
                        if (board[(int)position.Y, (int)i] != null)
                        {
                            break;
                        }
                        if (i == move.X - 1)
                        {
                            validMoves.Add(move);
                        }
                    }
                    if (move.X - position.X == 1)
                    {
                        validMoves.Add(move);
                    }
                }
                // ---------------------------------------------------
                if (position.X > move.X && position.Y > move.Y)
                {
                    for (int i = 1; i < position.Y - move.Y; i++)
                    {
                        if (board[(int)move.Y + i, (int)move.X + i] != null)
                        {
                            break;
                        }
                        if (move.X + i == position.X - 1 && move.Y + i == position.Y - 1)
                        {
                            validMoves.Add(move);
                        }
                    }
                    if (position.X - move.X == 1 && position.Y - move.Y == 1)
                    {
                        validMoves.Add(move);
                    }
                }

                if (position.X < move.X && position.Y < move.Y)
                {
                    for (int i = 1; i < move.Y - position.Y; i++)
                    {
                        if (board[(int)position.Y + i, (int)position.X + i] != null)
                        {
                            break;
                        }
                        if (position.X + i == move.X - 1 && position.Y + i == move.Y - 1)
                        {
                            validMoves.Add(move);
                        }
                    }
                    if (move.X - position.X == 1 && move.Y - position.Y == 1)
                    {
                        validMoves.Add(move);
                    }
                }

                if (position.X > move.X && position.Y < move.Y)
                {
                    for (int i = 1; i < position.X - move.X; i++)
                    {
                        if (board[(int)position.Y + i, (int)position.X - i] != null)
                        {
                            break;
                        }
                        if (move.X + i == position.X - 1 && position.Y + i == move.Y - 1)
                        {
                            validMoves.Add(move);
                        }
                    }
                    if (position.X - move.X == 1 && move.Y - position.Y == 1)
                    {
                        validMoves.Add(move);
                    }
                }

                if (position.X < move.X && position.Y > move.Y)
                {
                    for (int i = 1; i < position.Y - move.Y; i++)
                    {
                        if (board[(int)move.Y + i, (int)move.X - i] != null)
                        {
                            break;
                        }
                        if (position.X + i == move.X - 1 && move.Y + i == position.Y - 1)
                        {
                            validMoves.Add(move);
                        }
                    }
                    if (move.X - position.X == 1 && position.Y - move.Y == 1)
                    {
                        validMoves.Add(move);
                    }
                }
            }
            return validMoves;
        }

        public List<Point> DeleteFencedOffMovesSecondVariant(List<Point> possibleMoves, Point position, Figure[,] board)
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

        public List<Point> EditPawnsMoves(List<Point> possibleMoves, Point position, Figure[,] board, Move lastMove)
        {
            List<Point> validMoves = new List<Point>();
            if (board[(int)position.Y, (int)position.X].GetType() == Figures.Pawn)
            {
                foreach (Point move in possibleMoves)
                {
                    if (Math.Abs(move.Y - position.Y) == 2)
                    {
                        if (CheckBrokenFieldMove(position, board))
                        {
                            validMoves.Add(move);
                        }
                        continue;
                    }
                    if (Math.Abs(move.X - position.X) == 1)
                    {
                        if (CheckPawnsTakeFigure(position, board, move))
                        {
                            validMoves.Add(move);
                        }
                        if (CheckEnPassant(position, board, move, lastMove))
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

        public bool CheckBrokenFieldMove(Point position, Figure[,] board)
        {
            if (FiguresStartPosition.Board[(int)position.Y, (int)position.X] != null &&
                FiguresStartPosition.Board[(int)position.Y, (int)position.X].Color == board[(int)position.Y, (int)position.X].Color &&
                board[(int)position.Y, (int)position.X].GetType() == FiguresStartPosition.Board[(int)position.Y, (int)position.X].GetType())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckPawnsTakeFigure(Point position, Figure[,] board, Point move)
        {
            if (board[(int)move.Y, (int)move.X] != null &&
                board[(int)position.Y, (int)position.X].Color != board[(int)move.Y, (int)move.X].Color)
            {
                return true;
            }
            return false;
        }

        public bool CheckEnPassant(Point position, Figure[,] board, Point move, Move lastMove)
        {
            if (board[(int)lastMove.EndPosition.Y, (int)lastMove.EndPosition.X] != null &&
                board[(int)lastMove.EndPosition.Y, (int)lastMove.EndPosition.X].Color != board[(int)position.Y, (int)position.X].Color &&
                board[(int)lastMove.EndPosition.Y, (int)lastMove.EndPosition.X].GetType() == Figures.Pawn &&
                move.Y == ((lastMove.EndPosition.Y > lastMove.StartPosition.Y) ? lastMove.StartPosition.Y + 1 : lastMove.EndPosition.Y + 1) &&
                move.X == lastMove.EndPosition.X)
            {
                EatenFigurePosition = lastMove.EndPosition;
                return true;
            }
            else
            {
                return false;
            }
        }

        public Point GetEatenFigurePosition()
        {
            return EatenFigurePosition;
        }

        public List<Point> CheckCastle(List<Point> possibleMoves, Point position, Figure[,] board, Player currentPlayer)
        {
            List<Point> validMoves = new List<Point>();
            foreach (Point move in possibleMoves)
            {
                if (currentPlayer.GetLongCastleAbility() &&
                   board[(int)position.Y, (int)position.X].GetType() == Figures.King &&
                   Math.Abs(move.X - position.X) == 2 &&
                   board[(int)move.Y, (int)move.X - 1] == null &&
                   board[(int)move.Y, (int)move.X + 1] == null)
                {
                    validMoves.Add(move);
                }
                if (currentPlayer.GetShortCastleAbility() &&
                   board[(int)position.Y, (int)position.X].GetType() == Figures.King &&
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
            return validMoves;
        }

        public bool CheckIsCheck(Player atacker, Player defender, Move lastMove, Figure[,] board)
        {
            for (int i = 0; i < 7; i ++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if (board[j, i] != null &&
                        board[j,i].Color == atacker.GetFigureColor())
                    {
                        atacker.RemoveCheck();
                        List<Point> Moves = GetPossibleMoves(lastMove.EndPosition, board, lastMove, atacker, defender);
                        foreach (Point move in Moves)
                        {
                            if (board[(int)move.Y, (int)move.X] != null &&
                                board[(int)move.Y, (int)move.X].GetType() == Figures.King)
                            {
                                defender.GiveCheck();
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public List<Point> FindCheckRemoveMove(List<Point> possibleMoves, Figure[,] LogicBoard, Point position, Player currentPlayer, Player uabledPlayer, Move lastMove)
        {
            List<Point> validMoves = new List<Point>();
            Figure[,] board = LogicBoard;

            foreach(Point move in possibleMoves)
            {
                board[(int)move.Y, (int)move.X] = board[(int)position.Y, (int)position.X];
                board[(int)position.Y, (int)position.X] = null;
                currentPlayer.RemoveCheck();
                if (!CheckIsCheck(uabledPlayer, currentPlayer, lastMove, board))
                {
                    validMoves.Add(move);
                }
                board[(int)position.Y, (int)position.X] = board[(int)move.Y, (int)move.X];
                board[(int)move.Y, (int)move.X] = null;
                currentPlayer.GiveCheck();
            }
            return validMoves;
        }
        //public bool CheckWhiteCheck(Figure[,] board)
        //{


        //    return false;
        //}
    }
}
