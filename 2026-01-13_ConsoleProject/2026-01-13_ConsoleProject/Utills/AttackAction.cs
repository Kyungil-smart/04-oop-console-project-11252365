
public class AttackAction : IBattleAction
{
    public void Execute(TrainerPokemon actor, TrainerPokemon target, Queue<string> messageQueue)
    {
        int damage = actor.Atk;

        messageQueue.Equals($"나의 {actor.BasePokemonData.Name}의  공격!");
        target.TakeDamage(damage);
        messageQueue.Enqueue($"{target.BasePokemonData.Name}는 {damage}의 데미지를 받았다.");
        messageQueue.Enqueue($"{target.BasePokemonData.Name}의 체력은 {target.Hp}로 떨어졌다.");
    }
}

