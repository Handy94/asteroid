namespace Asteroid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using HandyPackage;

    public partial class PlayerDataSaver
    {
        public void Save_HighScore()
        {
            _playerDataManager.TrySave<int>(PlayerDataKeys.INT_HIGH_SCORE, _playerData.highScore.Value);
        }
    }
}