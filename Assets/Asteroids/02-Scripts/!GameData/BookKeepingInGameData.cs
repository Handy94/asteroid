using UniRx;

namespace Asteroid
{
    public class BookKeepingInGameData
    {
        public IntReactiveProperty PlayerLife = new IntReactiveProperty(3);
        public IntReactiveProperty Score = new IntReactiveProperty(0);
        public IntReactiveProperty HighScore = new IntReactiveProperty(0);
    }

}
