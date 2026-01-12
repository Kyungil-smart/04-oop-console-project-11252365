public static class a
{
    private const int W = 10;
    private const int H = 10;

    // 커서 좌표 (0~9)
    private static int _x = 0;
    private static int _y = 0;

    
    
    public static void Run()
    {
        Console.CursorVisible = false;
        
        while (true)
        {
            Render();

            ConsoleKey key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    if (_x > 0) _x--;
                    break;

                case ConsoleKey.RightArrow:
                    if (_x < W - 1) _x++;
                    break;

                case ConsoleKey.UpArrow:
                    if (_y > 0) _y--;
                    break;

                case ConsoleKey.DownArrow:
                    if (_y < H - 1) _y++;
                    break;

                case ConsoleKey.Enter:
                    Console.SetCursorPosition(0, H + 2);
                    Console.WriteLine($"선택됨: ({_x}, {_y})");
                    Console.WriteLine("계속하려면 아무 키...");
                    Console.ReadKey(true);
                    break;

                case ConsoleKey.Escape:
                    return;
            }
        }
    }

    private static void Render()
    {
        Console.SetCursorPosition(0, 0);
        int cnt = 1;
        for (int y = 0; y < H; y++)
        {
            for (int x = 0; x < W; x++)
            {
                PokemonData p = PokemonCatalog.ById[cnt];
                bool isCursor = (x == _x && y == _y);
                
                if (isCursor)
                {
                    
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.Write("->");
                    Console.ResetColor();
                    
                }
                else
                {
                    
                    Console.Write($"{p.Name}  ");
                }
                cnt++;
            }
            Console.WriteLine();
        }

        Console.WriteLine();
        Console.WriteLine("방향키: 이동 | Enter: 선택 | Esc: 종료");
        Console.WriteLine($"현재 커서: ({_x}, {_y})");
    }
}
