using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFChessClient.Structures;
using static WPFChessClient.Pages.GamePlayPage;

namespace WPFChessClient.Logic
{
    static class FiguresStartPosition
    {
        public static Figure[,] Board;
        public static Figure[,] GetFiguresInStartPosition()
        {
            Board = new Figure[BoardDimensions.CellCount, BoardDimensions.CellCount];

            for (int i = 0; i < BoardDimensions.CellCount; i++) 
            {
                Board[6, i] = new Pawn(Figures.Pawn, FiguresColor.white);
                Board[1, i] = new Pawn(Figures.Pawn, FiguresColor.black);
            }

            Board[7, 4] = new King(Figures.King, FiguresColor.white);
            Board[0, 4] = new King(Figures.King, FiguresColor.black);
            Board[7, 3] = new Queen(Figures.Queen, FiguresColor.white);
            Board[0, 3] = new Queen(Figures.Queen, FiguresColor.black);
            Board[7, 2] = new Bishop(Figures.Bishop, FiguresColor.white);
            Board[7, 5] = new Bishop(Figures.Bishop, FiguresColor.white);
            Board[0, 2] = new Bishop(Figures.Bishop, FiguresColor.black);
            Board[0, 5] = new Bishop(Figures.Bishop, FiguresColor.black);
            Board[7, 1] = new Knight(Figures.Knight, FiguresColor.white);
            Board[7, 6] = new Knight(Figures.Knight, FiguresColor.white);
            Board[0, 1] = new Knight(Figures.Knight, FiguresColor.black);
            Board[0, 6] = new Knight(Figures.Knight, FiguresColor.black);
            Board[7, 0] = new Rook(Figures.Rook, FiguresColor.white);
            Board[7, 7] = new Rook(Figures.Rook, FiguresColor.white);
            Board[0, 0] = new Rook(Figures.Rook, FiguresColor.black);
            Board[0, 7] = new Rook(Figures.Rook, FiguresColor.black);

            return Board;
        }
    }
}
