using static System.Console;

namespace ConsoleApp.SnakeGame
{
    internal class Program
    {
        private const int MapWidth = 30;
        private const int MapHeight = 20;
        static void Main(string[] args)
        {
            SetWindowSize(MapWidth, MapHeight);
            SetBufferSize(MapWidth, MapHeight);
            CursorVisible = false;

            ReadKey();
        }
    }
    }
}
