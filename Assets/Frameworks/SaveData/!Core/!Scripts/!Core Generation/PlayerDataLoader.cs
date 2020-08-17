namespace Asteroid
{
    using HandyPackage;
    using UniRx.Async;

    public partial class PlayerDataLoader : IInitializable
    {
        private PlayerDataManager _playerDataManager;
        private PlayerData _playerData;
        public PlayerDataLoader()
        {
        }

        public PlayerDataLoader(PlayerDataManager playerDataManager, PlayerData playerData)
        {
            _playerDataManager = playerDataManager;
            _playerData = playerData;
        }

        public async UniTask Initialize()
        {
            LoadData();
            LoadDataCustom();
            await UniTask.CompletedTask;
        }

        public void LoadData()
        {
            LoadData_Asteroid();
        }

        public void LoadDataCustom()
        {
        }
    }
}