
public static class Trainer
{
    public static List<TrainerPokemon> Party { get; private set; } = new();
    // 
    public static int ActiveIndex { get; private set; } = 0;
    
    // 전투 시작할 포켓몬
    public static TrainerPokemon? ActivePokemon
    {
        get
        {
            if (Party.Count == 0) return null;
            if (ActiveIndex < 0) ActiveIndex = 0;
            if (ActiveIndex >= Party.Count) ActiveIndex = Party.Count - 1;
            
            return Party[ActiveIndex];
        }
    }
    
    // 포켓몬 추가, 6마리 초과면 false 
    public static bool AddPokemon(PokemonData basePokemon)
    {
        if (Party.Count >= 6) return false;
        Party.Add(new TrainerPokemon(basePokemon));
        return true;
    }
    
    // 게임 재시작을 위한 파티 클리어
    public static void PartyClear()
    {
        Party.Clear();
        ActiveIndex = 0;
    }
    
    // 포켓몬 변경 기능
    public static bool SetActive(int index)
    {
        if (index < 0 || index >= Party.Count) return false;
        ActiveIndex = index;
        return true;
    }


    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}