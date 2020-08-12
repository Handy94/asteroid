using HandyPackage;
using UniRx.Async;

namespace Asteroid
{
    public class ScoreSystem : IInitializable
    {
        private BookKeepingInGameData _bookKeepingInGameData;

        public UniTask Initialize()
        {
            _bookKeepingInGameData = DIResolver.GetObject<BookKeepingInGameData>();
            return UniTask.CompletedTask;
        }

        public void AddScore(int addition)
        {
            _bookKeepingInGameData.Score.Value += addition;
        }
    }

}
