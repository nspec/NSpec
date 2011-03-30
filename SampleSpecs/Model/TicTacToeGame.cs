using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleSpecs.Model
{
    public class TicTacToGame
    {
        public TicTacToGame()
        {
            Board = new string[3, 3] 
            { 
                { "", "", "" }, 
                { "", "", "" }, 
                { "", "", "" } 
            };
        }

        public bool Done { get; private set; }
        public string[,] Board { get; set; }

        public void Play(string xo, int row, int column)
        {
            if (string.IsNullOrEmpty(Board[row, column]) == false) throw new InvalidOperationException();

            Board[row, column] = xo;
            TestDone();
        }

        private void TestDone()
        {
            3.Times(i =>
            {
                new[] { "x", "o" }.Each(player =>
                {
                    CheckStraightColumn(i, player);
                    CheckStraightRow(i, player);
                });
            });

            CheckDiagonalLeft("x");
            CheckDiagonalLeft("o");
            CheckDiagonalRight("x");
            CheckDiagonalRight("o");

            CheckAllSquaresTaken();
        }

        private void CheckStraightColumn(int column, string xo)
        {
            if (Board[column, 0] == xo && Board[column, 1] == xo && Board[column, 2] == xo)
            {
                Done = true;
                Winner = xo;
            }
        }

        private void CheckStraightRow(int row, string xo)
        {
            if (Board[0, row] == xo && Board[1, row] == xo && Board[2, row] == xo)
            {
                Done = true;
                Winner = xo;
            }
        }

        private void CheckDiagonalLeft(string xo)
        {
            if (Board[0, 0] == xo && Board[1, 1] == xo && Board[2, 2] == xo)
            {
                Done = true;
                Winner = xo;
            }
        }

        private void CheckDiagonalRight(string xo)
        {
            if (Board[2, 0] == xo && Board[1, 1] == xo && Board[0, 2] == xo)
            {
                Done = true;
                Winner = xo;
            }
        }

        private void CheckAllSquaresTaken()
        {
            var val = from string xo
                      in Board
                      where xo == string.Empty
                      select xo;

            if (val.Count() == 0)
            {
                Done = true;
                if (string.IsNullOrEmpty(Winner)) Draw = true;
            }
        }

        public bool Draw { get; private set; }
        public string Winner { get; private set; }
    }

    public static class extensions
    {
        public static void Times(this int number, Action<int> action)
        {
            for (int i = 0; i < number; i++)
            {
                action(i);
            }
        }

        public static void Each<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (T t in list)
            {
                action(t);
            }
        }
    }
}
