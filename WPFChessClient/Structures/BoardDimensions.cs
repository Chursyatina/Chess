using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFChessClient.Structures
{
    public struct BoardDimensions
    {
        public const double Border = 3;
        public const int CellCount = 8;
        public Rect FullBoard { get; set; }
        public Rect PlayBoard { get; set; }
        public Point CanvasSide { get; set; }
        public double CellSide { get; set; }

    }
}
