class  Program
{
    private static PokemonData _pokemonData;

    static void Main()
    {
        GameManager gameManager = new GameManager();
        gameManager.Run();
        // a.Run();
        // _pokemonData = PokemonCatalog.ById[0];
        // Console.WriteLine(_pokemonData.Name);
    }
}