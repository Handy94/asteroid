namespace Asteroid
{
    using HandyPackage;

    public partial class PlayerDataSaver
    {
        private PlayerDataManager _playerDataManager;
        private PlayerData _playerData;

        public PlayerDataSaver()
        {

        }

        public PlayerDataSaver(PlayerDataManager _playerDataManager, PlayerData _playerData)
        {
            this._playerDataManager = _playerDataManager;
            this._playerData = _playerData;
        }
    }
}
