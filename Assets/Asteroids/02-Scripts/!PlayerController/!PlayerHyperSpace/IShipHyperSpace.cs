namespace Asteroid
{
    public interface IShipHyperSpace
    {
        bool IsOnHyperSpace { get; }
        void DoHyperSpace(System.Action onDone);
    }

}
