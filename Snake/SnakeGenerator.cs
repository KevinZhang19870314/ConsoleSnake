using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snake
{
    /// <summary>
    /// Class SnakeGenerator
    /// </summary>
    public class SnakeGenerator
    {
        /// <summary>
        /// Generate a snake
        /// </summary>
        /// <param name="snake">The snake</param>
        /// <returns>New snake</returns>
        public static Snake GenerateSnake(Snake snake)
        {
            Snake tempSnake = new Snake();
            tempSnake = snake;
            return tempSnake;
        }
        /// <summary>
        /// Generate a snake
        /// </summary>
        /// <param name="head">The snake's head</param>
        /// <param name="body">The snake's body</param>
        /// <param name="body">The snake's tail</param>
        /// <returns>New snake</returns>
        public static Snake GenerateSnake(Point head, Point[] body , Point tail)
        {
            Snake tempSnake = new Snake();
            tempSnake.Head = head;
            tempSnake.Body = body;
            tempSnake.Tail = tail;
            return tempSnake;
        }
    }
}
