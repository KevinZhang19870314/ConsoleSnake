using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snake
{
    /// <summary>
    /// Class Food
    /// </summary>
    public class Food
    {
        private Point position = null;
        /// <summary>
        /// Gets or sets the food's position
        /// </summary>
        public Point Position
        {
            get { return position; }
            set { position = value; }
        }
    }
}
