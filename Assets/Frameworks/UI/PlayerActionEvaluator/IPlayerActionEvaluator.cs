namespace HandyPackage
{
    public interface IPlayerActionEvaluator
    {
        bool EvaluatePlayerAction(PlayerAction playerAction, object payload);
    }
}
