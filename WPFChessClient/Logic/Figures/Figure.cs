﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static WPFChessClient.Pages.GamePlayPage;

namespace WPFChessClient.Logic
{
    public abstract class Figure
    {
        public Figures Name { get; set; }
        public FiguresColor Color { get;  set; }

        public Figure(Figures name, FiguresColor color)
        {
            Name = name;
            Color = color;
        }

        public Figure()
        {

        }

        public abstract List<Point> GetPossibleMoves(Point position);

        public abstract Figures GetType();
        
    }
}
