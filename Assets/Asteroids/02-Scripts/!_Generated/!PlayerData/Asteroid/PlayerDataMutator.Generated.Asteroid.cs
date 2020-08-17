namespace Asteroid
{
    using UniRx;
    using System;
    using System.Collections.Generic;
    using HandyPackage;

    public partial class PlayerDataMutator
    {
        public void Set_HighScore(int value)
        {
            _playerData.highScore.Value = value;
        }

        public void Add_HighScore(int value)
        {
            Set_HighScore(_playerData.highScore.Value + value);
        }

        public void Use_HighScore(int value)
        {
            Set_HighScore(_playerData.highScore.Value - value);
        }
    }
}