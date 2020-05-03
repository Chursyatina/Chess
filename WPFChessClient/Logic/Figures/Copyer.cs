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
            int height = array.GetLength(0);
            int width = array.GetLength(1);
            Figure[,] figures = new Figure[height, width];
            //for (int i = 0; i < height; i++)
            //{
            //    for (int j = 0; j < width; j++)
            //    {
            //        Figure figure = new
            //    }
            //}

            Array.Copy(array, figures, array.Length);
            return figures;
        }
    }
}
