using HandyPackage;
using UniRx.Async;

namespace Asteroid
{
    public class HighScoreManager : IInitializable
    {
        private PlayerDataGetter _playerDataGetter;
        private PlayerDataMutator _playerDataMutator;

        public UniTask Initialize()
        {
            _playerDataGetter = DIResolver.GetObject<PlayerDataGetter>();
            _playerDataMutator = DIResolver.GetObject<PlayerDataMutator>();

            return UniTask.CompletedTask;
        }

        public int GetCurrentHighScore()
        {
            return _playerDataGetter.GetValue_HighScore();
        }

        public bool IsNewHighScore(int value)
        {
            return value > GetCurrentHighScore();
        }

        private void SetHighScore(int value)
        {
            _playerDataMutator.Set_HighScore(value);
        }

        public bool TryToSetNewHighScore(int newScore)
        {
            bool isNewHighScore = IsNewHighScore(newScore);
            if (isNewHighScore)
            {
                SetHighScore(newScore);
            }
            return isNewHighScore;
        }
    }

}
