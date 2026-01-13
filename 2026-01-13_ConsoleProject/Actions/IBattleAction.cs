// 아이템, 스킬 등 플레이어의 행동(나중에...)
public interface IBattleAction
{
    public void Execute(TrainerPokemon actor, TrainerPokemon target, Queue<string> messageQueue);
}