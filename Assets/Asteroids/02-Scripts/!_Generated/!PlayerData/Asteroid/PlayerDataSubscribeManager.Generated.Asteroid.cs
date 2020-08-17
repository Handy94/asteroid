namespace Asteroid
{
    using UniRx;
    using HandyPackage;

    public partial class PlayerDataSubscribeManager
    {
        public void SubscribeForSave_Asteroid()
        {
            _playerData.highScore.SubscribeToPersistence(PlayerDataKeys.INT_HIGH_SCORE, disposables, false);
        }
    }
}