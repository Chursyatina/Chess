using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFChessClient.Logic
{
    static class Copyer
    {
        public static Figure[,] GetCopy(Figure[,] array)
        {
            Figure[,] figures = new Figure[8, 8];
            figures = array;
            return figures;
        }
    }
}
