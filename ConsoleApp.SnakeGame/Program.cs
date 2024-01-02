using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using static System.Console;
using static System.Formats.Asn1.AsnWriter;

namespace ConsoleApp.SnakeGame
{
    internal class Program
    {
        private const int MapWidth = 30;
        private const int MapHeight = 20;
        private const int ScreenWidth = MapWidth * 3;
        private const int ScreenHeight = MapHeight * 3;

        private const ConsoleColor BorderColor = ConsoleColor.Gray;

        private const ConsoleColor HeadColor = ConsoleColor.DarkBlue;
        private const ConsoleColor BodyColor = ConsoleColor.Cyan;
        private const int InitX = 10;
        private const int InitY = 5;

        private const int FrameMs = 200;

        private static readonly Random Random = new Random();

        private const ConsoleColor FoodColor = ConsoleColor.Green;

        private static int Score = 0;

        private const int BeepFrecuency = 1200;
        private const int BeepDuration = 200;
        private const int BeepGameOverFrequency = 200;
        private const int BeepGameOverDuration = 600;
        static void Main(string[] args)
        {
            SetWindowSize(ScreenWidth, ScreenHeight);
            SetBufferSize(ScreenWidth, ScreenHeight);
            CursorVisible = false;

            while (true)
            {
                StartGame();

                Thread.Sleep(2000);

                ReadKey();
            }
        }

        static void StartGame()
        {
            Clear();

            DrawBorder();

            Snake snake = new Snake(InitX, InitY, HeadColor, BodyColor);

            Pixel food = GenFood(snake);
            food.Draw();

            Direction currentMovement = Direction.Right;

            int lagMs = 0;
            Stopwatch sw = new Stopwatch();
            while (true)
            {
                sw.Restart();

                Direction oldMovement = currentMovement;

                while (sw.ElapsedMilliseconds <= FrameMs - lagMs)
                {
                    if (oldMovement == currentMovement)
                        currentMovement = ReadMovement(currentMovement);
                }

                sw.Restart();

                if (snake.Head.X == food.X && snake.Head.Y == food.Y)
                {
                    snake.Move(currentMovement, true);
                    Score++;

                    food = GenFood(snake);
                    food.Draw();

                    Task.Run(() => Beep(BeepFrecuency, BeepDuration));
                }
                else
                    snake.Move(currentMovement);

                if (snake.Head.X == MapWidth - 1
                    || snake.Head.Y == MapHeight - 1
                    || snake.Head.X == 0
                    || snake.Head.Y == 0
                    || snake.Body.Any(b => b.X == snake.Head.X && b.Y == snake.Head.Y))
                    break;

                lagMs = (int)sw.ElapsedMilliseconds;
            }

            snake.Clear();
            SetCursorPosition(ScreenWidth / 2, ScreenHeight / 3);
            WriteLine($"Game Over, score: {Score}");

            Task.Run(() => Beep(BeepGameOverFrequency, BeepGameOverDuration));
        }

        static Pixel GenFood(Snake snake)
        {
            Pixel food;

            do
            {
                food = new Pixel(Random.Next(1, MapWidth - 2), Random.Next(1, MapHeight - 2), FoodColor);
            } while (snake.Head.X == food.X && snake.Head.Y == food.Y
                || snake.Body.Any(b => b.X == food.X && b.Y == food.Y));

            return food;
        }

        static Direction ReadMovement(Direction currentDir)
        {
            if (!KeyAvailable)
                return currentDir;

            ConsoleKey key = ReadKey(true).Key;

            currentDir = key switch
            {
                ConsoleKey.UpArrow when currentDir != Direction.Down => Direction.Up,
                ConsoleKey.DownArrow when currentDir != Direction.Up => Direction.Down,
                ConsoleKey.RightArrow when currentDir != Direction.Left => Direction.Right,
                ConsoleKey.LeftArrow when currentDir != Direction.Right => Direction.Left,
                _ => currentDir
            };

            return currentDir;
        }

        static void DrawBorder()
        {
            for(int i = 0; i < MapWidth; i++)
            {
                new Pixel(i, 0, BorderColor).Draw();
                new Pixel(i, MapHeight - 1, BorderColor).Draw();
            }
            
            for(int i = 0; i < MapHeight; i++)
            {
                new Pixel(0, i, BorderColor).Draw();
                new Pixel(MapWidth - 1, i, BorderColor).Draw();
            }
        }
    }
}
