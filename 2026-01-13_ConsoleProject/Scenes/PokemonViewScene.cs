
public sealed class  PokemonViewScene : Scene
{
    private int _pokemonId;
    
    public void Set(int id)
    {
        _pokemonId = id;
    }
    
    public override void Enter()
    {
        
    }

    public override void Update()
    {
        if (InputManager.IsCurrentKey(ConsoleKey.Q) || InputManager.IsCurrentKey(ConsoleKey.W))
        {
            SceneManager.ChangePrevScene();
        }
    }

    public override void Render()
    {
        Console.Clear();
        PokedexAscii.PrintPokemon(_pokemonId);

        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        "Q 또는 W : 돌아가기".Print(ConsoleColor.Yellow);
    }

    public override void Exit()
    {
        
    }
}