
public class UIFrame
{
    public Rectangle Rect { get; set; }
    
    public int X => Rect.X;
    public int Y => Rect.Y;
    public int Width => Rect.Width;
    public int Height => Rect.Height;

    public UIFrame(int x = 0, int y = 0, int width = 110, int height = 30)
    {
        Rect = new Rectangle(x, y, width, height);
    }
    
    public void Draw() => Rect.Draw();
    

}