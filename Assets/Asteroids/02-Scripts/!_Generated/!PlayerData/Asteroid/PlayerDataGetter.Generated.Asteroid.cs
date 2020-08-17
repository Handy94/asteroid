namespace Asteroid
{
    using UniRx;

    public partial class PlayerDataGetter
    {
        public IntReactiveProperty GetReactiveProperty_HighScore()
        {
            return _playerData.highScore;
        }

        public int GetValue_HighScore()
        {
            return GetReactiveProperty_HighScore().Value;
        }
    }
}
