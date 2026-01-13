public class PokemonSelectionScene : Scene
{
    // 포켓몬 리스트 출력 가로(GridW), 세로(GridH)
    private static readonly int GridW = 5;
    private static readonly int GridH = 10;
    
    private List<PokemonData> _pokemons = new();
    private Rectangle _maxArea;
    private Rectangle _pokemonArea;
    private Rectangle _infoArea;
    private Rectangle _helpArea;

    private int _cursorX;
    private int _cursorY;
    
    
    public PokemonSelectionScene() => Init();

    private void Init()
    {
        _cursorX = 0;
        _cursorY = 0;
    }
    
    public override void Enter()
    {
        BuildAreas();
        _pokemons = PokemonCatalog.ById.Values.ToList();

    }

    public override void Update()
    {
        if (InputManager.IsCurrentKey(ConsoleKey.UpArrow))         MoveCursor(0, -1);
        else if (InputManager.IsCurrentKey(ConsoleKey.DownArrow))  MoveCursor(0, 1);
        else if (InputManager.IsCurrentKey(ConsoleKey.LeftArrow))  MoveCursor(-1, 0);
        else if (InputManager.IsCurrentKey(ConsoleKey.RightArrow)) MoveCursor(1, 0);
        else if (InputManager.IsCurrentKey(ConsoleKey.Escape)) SceneManager.Change(SceneType.Title);
        else if (InputManager.IsCurrentKey(ConsoleKey.Enter))
        {
            PokemonData? poke = GetCurrentPokemon();
            if (poke == null) return;
            Trainer.PartyClear();
            
            // 포켓몬 보유중이면 배틀씬으로
            bool flag = Trainer.AddPokemon(poke);
            if (!flag) return;
           SceneManager.Change(SceneType.Battle);
        }
    }

    public override void Render()
    {
        // Area 
        _maxArea.Draw();
        _pokemonArea.Draw();
        _infoArea.Draw();
        _helpArea.Draw();
        
        // 글자 출력
        Console.SetCursorPosition(_pokemonArea.X + 2, _pokemonArea.Y -1 );
        "포켓몬 목록".Print(ConsoleColor.Yellow);
        Console.SetCursorPosition(_infoArea.X + 2, _infoArea.Y - 1);
        "포켓몬 정보".Print(ConsoleColor.Yellow);
        Console.SetCursorPosition(_helpArea.X + 2, _helpArea.Y + 1);
        Console.Write("조작: →↑↓← 방향키 이동 / Enter 선택 / Esc 뒤로");
        
        DrawPokemonGrid();
        DrawInfo();
    }

    public override void Exit()
    {
        
    }

    private void BuildAreas()
    {
        // 외각
        int maxAreaW = 110;
        int maxAreaH = 30;
        _maxArea = new Rectangle(0,0,maxAreaW, maxAreaH);

        // 포켓몬 목록
        // 
        int listX = 2;
        int listY = 2;
        int listW = 66;
        int listH = 14;
        _pokemonArea = new Rectangle(listX, listY, listW, listH);

        // 상세 정보
        int infoX = listX + listW + 2;
        int infoY = listY;
        int infoW = maxAreaW - listW - 6; // 
        int infoH = 14;
        _infoArea = new Rectangle(infoX, infoY, infoW, infoH);

        // 조작 방법
        int helpX =  2;
        int helpY = listY + listH + 1;
        int helpW = maxAreaW - 4;
        int helpH = 6;
        _helpArea = new Rectangle(helpX, helpY, helpW, helpH);

        // 최하단에 제한된 코스트 만큼 포켓몬 데려가기 max6마리, 최소1마리
        // 지금은 1개 선택하면 배틀씬으로
    }
    
    // 포켓몬 리스트 출력
    private void DrawPokemonGrid()
    {
        int stringWidth = 12;
        int rowSpacing = 1;
        
        int startX = _pokemonArea.X + 2;
        int startY = _pokemonArea.Y + 2;

        for (int y = 0; y < GridH; y++)
        {
            for (int x = 0; x < GridW; x++)
            {
                int idx = y * GridW + x;
                int printX = startX + x * stringWidth;
                int printY = startY + y * rowSpacing;
                
                Console.SetCursorPosition(printX, printY);
                
                PokemonData pokemon = _pokemons[idx];
                
                string name = pokemon.Name;

                //int pad = stringWidth - name.GetTextWidth();
                
                bool isCursor = (x== _cursorX && y == _cursorY);
                if (isCursor)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    
                }
                name.Print();
                Console.ResetColor();
            }
            
        }
    }
    
    // 현재 선택 포켓몬
    private PokemonData? GetCurrentPokemon()
    {
        int index = _cursorY * GridW + _cursorX;
        
        if (index < 0 || index >= _pokemons.Count)  return null;
        return _pokemons[index]; 
    }
    
    // 포켓몬 상세 정보
    private void DrawInfo()
    {
        PokemonData? poke = GetCurrentPokemon();
        if (poke == null) return;
        
        int x = _infoArea.X + 2;
        int y = _infoArea.Y + 1;

        Console.SetCursorPosition(x, y);     $"ID     : No. {poke.Id:0000}".Print();
        Console.SetCursorPosition(x, y + 2); $"Name   : {poke.Name}".Print();
        Console.SetCursorPosition(x, y + 3); $"HP     : {poke.Hp}".Print();
        Console.SetCursorPosition(x, y + 4); $"Atk    : {poke.Atk}".Print();
        Console.SetCursorPosition(x, y + 5); $"Def    : {poke.Def}".Print();
    }

    // 포켓몬 목록 커서 조작
    private void MoveCursor(int x, int y)
    {
        _cursorX += x;
        _cursorY += y;
        
        if (_cursorX < 0) _cursorX = 0;
        else if (_cursorX >= GridW) _cursorX = GridW - 1;
        
        if (_cursorY < 0) _cursorY = 0;
        else if (_cursorY >= GridH) _cursorY = GridH - 1;
        
        int idx = _cursorY * GridW + _cursorX;
        if (idx >= _pokemons.Count) 
        {
           // int last = _pokemons.Count - 1;
            _cursorX = (_pokemons.Count - 1) % GridW;
            _cursorY = (_pokemons.Count - 1) / GridW;
        }
        

    }
    
    
}