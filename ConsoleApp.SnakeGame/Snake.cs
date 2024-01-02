namespace ConsoleApp.SnakeGame
{
    public class Snake
    {
        private readonly ConsoleColor HeadColor;
        private readonly ConsoleColor BodyColor;

        public Pixel Head { get; private set; }
        public Queue<Pixel> Body { get; } = new Queue<Pixel>();

        public Snake(int initX, int initY, 
            ConsoleColor headColor, ConsoleColor bodyColor, 
            int bodyLength = 3)
        {
            HeadColor = headColor;
            BodyColor = bodyColor;
            Head = new Pixel(initX, initY, HeadColor);

            for(int i = bodyLength - 1; i >= 0; i--)
            {
                Body.Enqueue(new Pixel(Head.X - i - 1, initY, BodyColor));
            }

            Draw();
        }

        public void Move(Direction dir, bool eat = false)
        {
            Clear();

            Body.Enqueue(new Pixel(Head.X, Head.Y, BodyColor));

            if(!eat)
                Body.Dequeue();

            Head = dir switch
            {
                Direction.Right => new Pixel(Head.X + 1, Head.Y, HeadColor),
                Direction.Left => new Pixel(Head.X - 1, Head.Y, HeadColor),
                Direction.Up => new Pixel(Head.X, Head.Y - 1, HeadColor),
                Direction.Down => new Pixel(Head.X, Head.Y + 1, HeadColor),
                _ => Head
            };
            Draw();
        }

        public void Draw()
        {
            Head.Draw();

            foreach(Pixel p in Body)
            {
                p.Draw();
            }
        }

        public void Clear()
        {
            Head.Clear();

            foreach(Pixel p in Body)
            {
                p.Clear();
            }
        }
    }
}
