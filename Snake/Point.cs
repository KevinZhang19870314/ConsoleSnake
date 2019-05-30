using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snake
{
    /// <summary>
    /// Class Point
    /// </summary>
    public class Point : IComparable<Point>
    {
        private int x = 0;
        /// <summary>
        /// Gets or sets the x
        /// </summary>
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        private int y = 0;
        /// <summary>
        /// Gets or sets the y
        /// </summary>
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        /// <summary>
        /// Compare the two point
        /// </summary>
        /// <param name="other">Other point</param>
        /// <returns>-1 if x < other.X && y < other.Y , 0 if x == other.X && y == other.Y , otherwise 1</returns>
        public int CompareTo(Point other)
        {
            if (x < other.X && y < other.Y)
            {
                return -1;
            }
            else if (x == other.X && y == other.Y)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}
