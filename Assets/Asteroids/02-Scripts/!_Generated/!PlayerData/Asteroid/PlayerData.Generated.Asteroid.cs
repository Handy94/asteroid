namespace Asteroid
{
    using UniRx;
    using HandyPackage;

    public partial class PlayerData
    {
        public IntReactiveProperty highScore = new IntReactiveProperty();
    }
}