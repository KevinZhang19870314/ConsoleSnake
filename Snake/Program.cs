using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snake
{
    /// <summary>
    /// Main class
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args"></param>
        static void Main()
        {
            try
            {
                Console.Title = "Snake";
                SnakeHelper snakeHelper = new SnakeHelper();
                snakeHelper.InitConsoleUI();
                snakeHelper.StartGame();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
    }
}
