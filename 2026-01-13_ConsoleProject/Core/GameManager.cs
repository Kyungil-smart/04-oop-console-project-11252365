using System.Text;

public class GameManager
{
    public static bool IsRunning { get; private set; }

    public const string GameName = @"
                        .______      ______    __  ___  _______     __        ______     _______ 
                        |   _  \    /  __  \  |  |/  / |   ____|   |  |      /  __  \   /  _____|
                        |  |_)  |  |  |  |  | |  '  /  |  |__      |  |     |  |  |  | |  |  __  
                        |   ___/   |  |  |  | |    <   |   __|     |  |     |  |  |  | |  | |_ | 
                        |  |       |  `--'  | |  .  \  |  |____    |  `----.|  `--'  | |  |__| | 
                        | _|        \______/  |__|\__\ |_______|   |_______| \______/   \______| ";
                                                                        

    public void Run()
    {

        Console.WriteLine(GameName);
        GameStart();
        Init();

        while (IsRunning)
        {
            // 렌더링
            Console.Clear();
            
            //Console.WriteLine(GameName);
            SceneManager.Render();
            // 키입력 
            InputManager.GetUserInput();
            // 데이터처리
            SceneManager.Update();
        }
    }
    // 게임 start/stop
    public static void GameStart() => IsRunning = true;
    public static void GameStop() => IsRunning = false;
    
    // 초기화 작업
    private void Init()
    {
        Console.CursorVisible = false;
        Console.OutputEncoding = Encoding.UTF8;
        SceneManager.OnChangeScene += InputManager.ResetKey;
        
        SceneManager.AddScene(SceneType.Title, new TitleScene());
        SceneManager.AddScene(SceneType.Log, new LogScene());
        SceneManager.AddScene(SceneType.PokemonSeletion, new PokemonSelectionScene());
        SceneManager.AddScene(SceneType.Battle, new BattleScene());
        SceneManager.AddScene(SceneType.PokemonView, new PokemonViewScene());
        SceneManager.Change(SceneType.Title);
        
        
    }

}


