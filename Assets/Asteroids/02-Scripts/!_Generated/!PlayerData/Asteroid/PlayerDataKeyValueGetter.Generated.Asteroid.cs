namespace Asteroid
{
    using System;
    using System.Collections.Generic;
    using HandyPackage;

    public partial class PlayerDataKeyValueGetter
    {
        private void CreateGetterMapAsteroid()
        {
            getterMap[PlayerDataKeys.INT_HIGH_SCORE] = () => _playerDataGetter.GetValue_HighScore();
        }
    }
}