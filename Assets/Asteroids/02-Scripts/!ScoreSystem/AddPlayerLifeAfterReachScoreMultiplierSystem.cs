using HandyPackage;
using UniRx;
using UniRx.Async;

namespace Asteroid
{

    public class AddPlayerLifeAfterReachScoreMultiplierSystem : IInitializable, System.IDisposable
    {
        private BookKeepingInGameData _bookKeepingInGameData;
        private AsteroidGameSettings _asteroidGameSettings;

        private CompositeDisposable disposables = new CompositeDisposable();
        private int _prevScore = 0;

        public UniTask Initialize()
        {
            _bookKeepingInGameData = DIResolver.GetObject<BookKeepingInGameData>();
            _asteroidGameSettings = DIResolver.GetObject<AsteroidGameSettings>();

            _bookKeepingInGameData.Score.Subscribe(x =>
            {
                EvaluateScore(x);
            }).AddTo(disposables);

            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            disposables.Clear();
        }

        private void EvaluateScore(int score)
        {
            int prevMultiplier = _prevScore / _asteroidGameSettings.bonusLifeScoreMultiplierThreshold;
            int currMultiplier = score / _asteroidGameSettings.bonusLifeScoreMultiplierThreshold;

            if (currMultiplier > prevMultiplier)
            {
                _bookKeepingInGameData.PlayerLife.Value += (currMultiplier - prevMultiplier) * _asteroidGameSettings.bonusLifeAdd;
            }
            _prevScore = score;
        }
    }

}
