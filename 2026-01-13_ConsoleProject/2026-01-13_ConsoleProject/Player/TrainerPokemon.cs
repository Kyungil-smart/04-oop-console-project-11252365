public class TrainerPokemon
{
    public PokemonData BasePokemonData { get; }

    public int Level { get; private set; }
    public int Hp { get; private set; }

    public int MaxHp
    {
        get => BasePokemonData.Hp + (Level - 1) * 10;
        private set;
    }
    public int Atk
    {
        get  => BasePokemonData.Atk + (Level - 1) * 10;
        private set;
    }

    public int Def
    {
        get => BasePokemonData.Def + (Level - 1) * 10;
        private set;
    }
    
    public TrainerPokemon(PokemonData basePokemonData)
    {
        BasePokemonData = basePokemonData;

        Level = 1;
        Hp = MaxHp;
    }

    public void FullHeal()
    {
        Hp = MaxHp;
    }
    public void TakeDamage(int damage)
    {
        Hp -= damage;
        if (Hp <= 0) Hp = 0;
    }
    
    
    // 스테이지 올라갈때마다 레벨, 체력회복
    // 경험치 시스템은 추후 업데이트(우선 기능 구현)
    public void OnStageUp()
    {
        
        Level += 1;
        FullHeal();
        
    }

    
}