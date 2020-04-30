using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static WPFChessClient.Pages.GamePlayPage;

namespace WPFChessClient.Logic
{
    struct Move
    {
        public Point StartPosition { get; private set; }

        public Point EndPosition { get; private set; }

        public Figures Figure { get; private set; }

        public void SetStartPosition(Point startPosition)
        {
            StartPosition = startPosition;
        }
        public void SetEndPosition(Point endPosition)
        {
            EndPosition = endPosition;
        }
        public void SetFigure(Figures figure)
        {
            Figure = figure;
        }
    }
}

