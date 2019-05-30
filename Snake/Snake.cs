using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snake
{
    /// <summary>
    /// Class Snake
    /// </summary>
    public class Snake
    {
        private Point head = null;
        /// <summary>
        /// Gets or sets the snake's head
        /// </summary>
        public Point Head
        {
            get { return head; }
            set { head = value; }
        }
        private Point[] body = null;
        /// <summary>
        /// Gets or sets the snake's body
        /// </summary>
        public Point[] Body
        {
            get { return body; }
            set { body = value; }
        }
        private Point tail = null;
        /// <summary>
        /// Gets or sets the snake's tail
        /// </summary>
        public Point Tail
        {
            get { return tail; }
            set { tail = value; }
        }
    }
}
