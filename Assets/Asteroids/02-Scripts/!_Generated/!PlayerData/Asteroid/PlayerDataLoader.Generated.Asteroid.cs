namespace Asteroid
{
    using UniRx;
    using System;
    using System.Collections.Generic;
    using HandyPackage;

    public partial class PlayerDataLoader
    {
        public void LoadData_Asteroid()
        {
            _playerData.highScore.Value = _playerDataManager.TryLoad<int>(PlayerDataKeys.INT_HIGH_SCORE, default, false);
        }
    }
}