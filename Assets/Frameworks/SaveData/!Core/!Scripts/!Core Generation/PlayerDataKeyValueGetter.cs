namespace Asteroid
{
    using HandyPackage;
    using System;
    using System.Collections.Generic;
    using UniRx.Async;

    public partial class PlayerDataKeyValueGetter : IInitializable
    {
        private PlayerDataGetter _playerDataGetter;

        private static Dictionary<string, Func<object>> getterMap = new Dictionary<string, Func<object>>();

        public UniTask Initialize()
        {
            _playerDataGetter = DIResolver.GetObject<PlayerDataGetter>();
            InitMaps();
            InitCustomMaps();
            return UniTask.CompletedTask;
        }

        private void InitMaps()
        {

        }

        private void InitCustomMaps()
        {
        }

    }

}
