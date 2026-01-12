public class TitleScene : Scene
{
    private MenuList _titleMenu;
    public TitleScene() => Init();
    
    private void Init()
    {
        _titleMenu = new MenuList();
        _titleMenu.Add("게임 시작", StartGame);
        _titleMenu.Add("크레딧", ViewCredits);
        _titleMenu.Add("머넣지?", ViewCredits);
        _titleMenu.Add("게임 종료", ExitGame);
    }
    private void StartGame()
    {
        SceneManager.Change(SceneType.PokemonSeletion);

    }

    private void ViewCredits()
    {
        
    }

    private void ExitGame()
    {
        GameManager.GameStop();
    }
    
    
    
    public override void Enter()
    {
        _titleMenu.Reset();
        Debug.Log("타이틀 씬 진입");
    }

    public override void Update()
    {
        switch (InputManager.GetCurrentKey())
        {
            case ConsoleKey.UpArrow:
                _titleMenu.MoveIndex(-1);
                break;
            case ConsoleKey.DownArrow:
                _titleMenu.MoveIndex(+1);
                break;
            case ConsoleKey.Enter:
                _titleMenu.Select();
                break;
        }
    }

    public override void Render()
    {
        GameManager.GameName.Print();
        //PokedexAscii_01.PrintPokemon(6);
        
        _titleMenu.Render(50, 10);
        
        
    }

    public override void Exit()
    {
        
    }
 
    

    
    
    
}