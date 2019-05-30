using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;

namespace Snake
{
    /// <summary>
    /// Enum about game's level
    /// </summary>
    public enum Level
    {
        Beginner = 200,
        Intermediate = 100,
        Advanced = 50
    }
    /// <summary>
    /// Class SnakeHelper
    /// </summary>
    public class SnakeHelper
    {
        private const string headElement = @"> v < ^";
        private const string bodyElement = @"*";
        private const string tailElement = @"~";
        private const string foodElement = @"$";
        private const string wallElement = @".";
        private Level gameLevel = Level.Beginner;
        private string name = string.Empty;
        private int score = 0;
        private int uiWidth = Console.WindowWidth;
        /// <summary>
        /// Gets or sets UI's width
        /// </summary>
        public int UIWidth
        {
            get { return uiWidth; }
            set { uiWidth = value; }
        }
        private int uiHeight = Console.WindowHeight;
        /// <summary>
        /// Gets or sets UI's height
        /// </summary>
        public int UIHeight
        {
            get { return uiHeight; }
            set { uiHeight = value; }
        }
        /// <summary>
        /// Random init
        /// </summary>
        private Random random = new Random();
        private Food currentFood = null;
        /// <summary>
        /// Current food
        /// </summary>
        public Food CurrentFood
        {
            get { return currentFood; }
            set { currentFood = value; }
        }
        private Snake currentSnake = null;
        /// <summary>
        /// Current snake
        /// </summary>
        public Snake CurrentSnake
        {
            get { return currentSnake; }
            set { currentSnake = value; }
        }
        /// <summary>
        /// True if start game , otherwise false
        /// </summary>
        private bool isStartGame = true;
        /// <summary>
        /// Currently pressed key
        /// </summary>
        private ConsoleKey currentKey = ConsoleKey.A;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SnakeHelper()
        {

        }
        /// <summary>
        /// Init the console ui
        /// </summary>
        /// <param name="width">Console's width</param>
        /// <param name="height">Console's height</param>
        public void InitConsoleUI()
        {
            //Input the name
            Console.Clear();
            Console.WriteLine(@"Please input your name then press Enter : ");
            name = Console.ReadLine();
            if (string.IsNullOrEmpty(name.Trim()))
            {
                name = "Lazy user with no name!";
            }

            //Input the level
            Console.Clear();
            Console.WriteLine("Please choice the Level(default is 1): \n    1=>Beginner\n    2=>Intermediate\n    3=>Advanced\n");
            string inputLevel = Console.ReadLine();
            while (!Regex.IsMatch(inputLevel, @"^\d+"))
            {
                Console.WriteLine("Please input one of the number 1,2,3 : \n");
                inputLevel = Console.ReadLine();
            }
            int level = Convert.ToInt32(inputLevel);
            if (level == 1)
            {
                gameLevel = Level.Beginner;
            }
            else if (level == 2)
            {
                gameLevel = Level.Intermediate;
            }
            else if (level == 3)
            {
                gameLevel = Level.Advanced;
            }
            else
            {
                gameLevel = Level.Beginner;
            }

            Console.Clear();

            Console.CursorVisible = false;
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Green;
            InitFoodAndSnake();
        }
        Thread readKeyThread = null;
        /// <summary>
        /// Start game
        /// </summary>
        public void StartGame()
        {
            readKeyThread = new Thread(new ThreadStart(ReadKeyThread));
            readKeyThread.Start();

            while (isStartGame)
            {
                RefreshFoodAndSnake();
                Thread.Sleep((int)Enum.Parse(typeof(Level), gameLevel.ToString()));
            }
        }

        #region Private method
        /// <summary>
        /// Read key thread
        /// </summary>
        private void ReadKeyThread()
        {
            while (isStartGame)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                currentKey = key;
            }
        }
        /// <summary>
        /// Init food
        /// </summary>
        private void InitFood()
        {
            Food food = null;
            do
            {
                food = FoodGenerator.GenerateFood(GetRandomPoint());

            } while (IsSnakeCoverFood(currentSnake, food));
            if (food == null) throw new Exception("Food == null");
            currentFood = food;
        }
        /// <summary>
        /// Init snake
        /// Default snake head's length = 1 ,  body's length = 4 , tail's length = 1
        /// </summary>
        /// <param name="food">The food just inited</param>
        private void InitSnake(Food food)
        {
            Snake snake = null;
            do
            {
                Point head = GetRandomPoint();
                Point[] body = new Point[4];
                int i = 0;
                for (; i < body.Length; i++)
                {
                    body[i] = new Point() { X = (head.X + i + 1), Y = head.Y };
                }
                Point tail = new Point();
                tail.X = head.X + i + 1;
                tail.Y = head.Y;
                snake = SnakeGenerator.GenerateSnake(head, body, tail);
            } while (IsSnakeCoverFood(snake, food));
            if (snake == null) throw new Exception("Snake == null");
            currentSnake = snake;
        }
        /// <summary>
        /// Draw console ui
        /// </summary>
        private void DrawConsoleUI()
        {
            Console.Clear();
            Point tempPoint = null;

            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Name : " + name);
            Console.SetCursorPosition(0, 1);
            Console.WriteLine("Score : " + score);
            Console.SetCursorPosition(0, 2);
            Console.WriteLine("Press R to restart game after you losed the game, press E to exit game");
            for (int i = 0; i < uiWidth; i += 2)
            {
                Console.SetCursorPosition(i, 3);
                Console.Write(".");
            }

            for (int i = 2; i < uiHeight; i++)
            {
                for (int j = 0; j < uiWidth; j++)
                {
                    tempPoint = new Point();
                    tempPoint.X = j;
                    tempPoint.Y = i;

                    if (tempPoint.CompareTo(currentFood.Position) == 0)
                    {
                        Console.SetCursorPosition(j, i);
                        Console.Write(GetFoodElement());
                    }
                    else if (tempPoint.CompareTo(currentSnake.Head) == 0)
                    {
                        Console.SetCursorPosition(j, i);
                        Console.Write(GetHeadElement());
                    }
                    else if (IsSnakeBodyCoverPoint(currentSnake.Body, tempPoint))
                    {
                        Console.SetCursorPosition(j, i);
                        Console.Write(GetBodyElement());
                    }
                    else if (tempPoint.CompareTo(currentSnake.Tail) == 0)
                    {
                        Console.SetCursorPosition(j, i);
                        Console.Write(GetTailElement());
                    }
                }
            }
        }
        /// <summary>
        /// Move snake to specific position
        /// </summary>
        /// <param name="nextHead">The next head</param>
        private void MoveSnake(Point nextHead)
        {
            Snake snake = null;
            Point head = nextHead;
            Point[] body = new Point[currentSnake.Body.Length];
            body[0] = currentSnake.Head;
            int i = 1;
            for (; i < body.Length; i++)
            {
                body[i] = new Point() { X = currentSnake.Body[i - 1].X, Y = currentSnake.Body[i - 1].Y };
            }
            Point tail = new Point();
            tail.X = currentSnake.Body[i - 1].X;
            tail.Y = currentSnake.Body[i - 1].Y;
            snake = SnakeGenerator.GenerateSnake(head, body, tail);
            if (snake == null) throw new Exception("Snake == null");
            currentSnake = snake;
        }
        /// <summary>
        /// Eat food
        /// </summary>
        private void EatFood()
        {
            Snake snake = null;
            Point head = new Point();
            head.X = currentFood.Position.X;
            head.Y = currentFood.Position.Y;
            Point[] body = new Point[currentSnake.Body.Length + 1];
            body[0] = currentSnake.Head;
            int i = 1;
            for (; i < body.Length; i++)
            {
                body[i] = new Point() { X = currentSnake.Body[i - 1].X, Y = currentSnake.Body[i - 1].Y };
            }
            Point tail = new Point();
            tail.X = currentSnake.Tail.X;
            tail.Y = currentSnake.Tail.Y;
            snake = SnakeGenerator.GenerateSnake(head, body, tail);
            if (snake == null) throw new Exception("Snake == null");
            currentSnake = snake;
            score++;
        }
        /// <summary>
        /// Init the food and snake
        /// </summary>
        private void InitFoodAndSnake()
        {
            //Init food
            InitFood();
            //Init snake(default snake head's length = 1 ,  body's length = 4 , tail's length = 1)
            InitSnake(currentFood);
            //Draw food and snake and so on
            DrawConsoleUI();
        }
        /// <summary>
        /// Restart game
        /// </summary>
        private void RestartGame()
        {
            isStartGame = true;
            currentKey = ConsoleKey.A;
            InitFoodAndSnake();
            StartGame();
        }
        /// <summary>
        /// Stop current game
        /// </summary>
        private void StopCurrentGame()
        {
            isStartGame = false;
        }
        /// <summary>
        /// Execute when head cover wall or body or tail
        /// </summary>
        private void ExecuteWhenHeadCoverWallOrBodyOrTail()
        {
            string endWords = "Hi " + name + " , your total score is : " + score;
            Console.SetCursorPosition(0, uiHeight - 2);
            Console.WriteLine(endWords);
            Console.SetCursorPosition(0, uiHeight - 1);
            Console.WriteLine("Snake's head touched it's body or tail , you are losed this game!\nPress R to restart game OR Press E to exit");
            if (readKeyThread != null && readKeyThread.IsAlive)
            {
                readKeyThread.Abort();
            }
            currentKey = Console.ReadKey().Key;
            if (currentKey == ConsoleKey.R)
            {
                StopCurrentGame();
                RestartGame();
            }
            else if (currentKey == ConsoleKey.E)
            {
                isStartGame = false;
            }
            readKeyThread = new Thread(new ThreadStart(ReadKeyThread));
            readKeyThread.Start();
        }
        /// <summary>
        /// Refresh the console ui
        /// </summary>
        private void RefreshFoodAndSnake()
        {
            Point tempHead = null;
            switch (currentKey)
            {
                case ConsoleKey.UpArrow:
                    {
                        #region UpArrow
                        tempHead = new Point();
                        tempHead.X = currentSnake.Head.X;
                        tempHead.Y = currentSnake.Head.Y - 1;
                        break;
                        #endregion
                    }
                case ConsoleKey.DownArrow:
                    {
                        #region DownArrow
                        tempHead = new Point();
                        tempHead.X = currentSnake.Head.X;
                        tempHead.Y = currentSnake.Head.Y + 1;
                        break;
                        #endregion
                    }
                case ConsoleKey.LeftArrow:
                    {
                        #region LeftArrow
                        tempHead = new Point();
                        tempHead.X = currentSnake.Head.X - 1;
                        tempHead.Y = currentSnake.Head.Y;
                        break;
                        #endregion
                    }
                case ConsoleKey.RightArrow:
                    {
                        #region RightArrow
                        tempHead = new Point();
                        tempHead.X = currentSnake.Head.X + 1;
                        tempHead.Y = currentSnake.Head.Y;
                        break;
                        #endregion
                    }
                case ConsoleKey.E:
                    {
                        Console.SetCursorPosition(0, uiHeight - 1);
                        Console.WriteLine("\nPress Enter to exit");
                        isStartGame = false;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            if (tempHead == null) return;

            if (IsHeadCoverSnakeNeck(tempHead))
            {
                return;
            }
            if (IsHeadCoverBodyOrTail(tempHead) || IsHeadCoverWall(tempHead))
            {
                ExecuteWhenHeadCoverWallOrBodyOrTail();
                return;
            }
            if (IsHeadCoverFood(tempHead, currentFood))
            {
                //Eat food
                EatFood();
                //Init food
                InitFood();
            }
            else
            {
                //Move snake
                MoveSnake(tempHead);
            }
            //Draw UI
            DrawConsoleUI();
        }
        /// <summary>
        /// Check the snake's head cover wall or not
        /// </summary>
        /// <param name="nextHead">The next head moved to</param>
        /// <returns>True if next head cover wall , otherwise false</returns>
        private bool IsHeadCoverWall(Point nextHead)
        {
            if (nextHead.X < 0 || nextHead.Y < 4 || nextHead.X > uiWidth || nextHead.Y > uiHeight)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Check if the next head cover current snake's neck or not
        /// </summary>
        /// <param name="nextHead">The next head</param>
        /// <returns>True if head cover snake's neck , otherwise false</returns>
        private bool IsHeadCoverSnakeNeck(Point nextHead)
        {
            if (nextHead.CompareTo(currentSnake.Body[0]) == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Check the next head cover current snake or not
        /// </summary>
        /// <param name="nextHead">The next head moved to</param>
        /// <returns>True if next head cover current snake's body or tail , otherwise false</returns>
        private bool IsHeadCoverBodyOrTail(Point nextHead)
        {
            if (nextHead.CompareTo(currentSnake.Tail) == 0)
            {
                return true;
            }
            for (int i = 0; i < currentSnake.Body.Length; i++)
            {
                if (nextHead.CompareTo(currentSnake.Body[i]) == 0)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Check the head cover the food or not
        /// </summary>
        /// <param name="head">The head</param>
        /// <param name="food">The food</param>
        /// <returns>True if head cover food , otherwise false</returns>
        private bool IsHeadCoverFood(Point head, Food food)
        {
            if (head == null) return false;
            if (food == null) return false;
            if (head.CompareTo(food.Position) == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Check the snake's body cover point or not
        /// </summary>
        /// <param name="body">The snake's body</param>
        /// <param name="point">The point</param>
        /// <returns>True if body cover point , otherwise false</returns>
        private bool IsSnakeBodyCoverPoint(Point[] body, Point point)
        {
            if (point == null) return false;
            foreach (var item in body)
            {
                if (item.CompareTo(point) == 0)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Check the snake cover food or not
        /// </summary>
        /// <param name="snake">The snake object</param>
        /// <param name="food">The food object</param>
        /// <returns>True if snake cover food , otherwise false</returns>
        private bool IsSnakeCoverFood(Snake snake, Food food)
        {
            if (snake == null) return false;
            if (food == null) return false;
            if (snake.Head.CompareTo(food.Position) == 0)
            {
                return true;
            }
            foreach (var item in snake.Body)
            {
                if (item.CompareTo(food.Position) == 0)
                {
                    return true;
                }
            }
            if (snake.Tail.CompareTo(food.Position) == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Get the random point
        /// </summary>
        /// <returns>Random point</returns>
        private Point GetRandomPoint()
        {
            if (random == null)
            {
                random = new Random();
            }
            Point tempPoint = new Point();
            tempPoint.X = random.Next(uiWidth - 10) + 5;
            tempPoint.Y = random.Next(uiHeight - 10) + 5;
            return tempPoint;
        }
        /// <summary>
        /// Get the head element
        /// </summary>
        /// <returns>New element</returns>
        private string GetHeadElement()
        {
            string[] tempHeadStr = headElement.Split(' ');
            try
            {
                return tempHeadStr[(int)Enum.Parse(typeof(ConsoleKey), currentKey.ToString()) - 37];
            }
            catch
            {
                return tempHeadStr[0];
            }
        }
        /// <summary>
        /// Get the body element
        /// </summary>
        /// <returns>New element</returns>
        private string GetBodyElement()
        {
            return bodyElement;
        }
        /// <summary>
        /// Get the tail element
        /// </summary>
        /// <returns>New element</returns>
        private string GetTailElement()
        {
            return tailElement;
        }
        /// <summary>
        /// Get the food element
        /// </summary>
        /// <returns>New element</returns>
        private string GetFoodElement()
        {
            return foodElement;
        }
        /// <summary>
        /// Get the wall element
        /// </summary>
        /// <returns>New element</returns>
        private string GetWallElement()
        {
            return wallElement;
        }
        #endregion
    }
}
