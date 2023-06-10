namespace ClientApplication.Models
{
    public class Brick
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; } = 64;
        public int Height { get; } = 40;
        public bool IsDestroyed { get; set; } = false;
    }
}
