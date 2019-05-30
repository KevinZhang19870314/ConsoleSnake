using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snake
{
    /// <summary>
    /// Class FoodGenerator
    /// </summary>
    public class FoodGenerator
    {
        /// <summary>
        /// Generate a food 
        /// </summary>
        /// <param name="position">The food's position</param>
        /// <returns>New food</returns>
        public static Food GenerateFood(Point position)
        {
            Food tempFood = new Food();
            tempFood.Position = position;
            return tempFood;
        }
        /// <summary>
        /// Generate a food
        /// </summary>
        /// <param name="x">The food position's coordinate x</param>
        /// <param name="y">The food position's coordinate y</param>
        /// <returns>New food</returns>
        public static Food GenerateFood(int x, int y)
        {
            Point tempPosition = new Point();
            tempPosition.X = x;
            tempPosition.Y = y;
            Food tempFood = new Food();
            tempFood.Position = tempPosition;
            return tempFood;
        }
    }
}
