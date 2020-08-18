namespace Asteroid
{
    using HandyPackage;

    public class GameStartPrioritySignal : PrioritySignal
    {
        public const int PRIORITY_SPAWN_PLAYER = 0;
        public const int PRIORITY_SPAWN_WAVE = 1;
        public const int PRIORITY_SETUP_SPAWN_ENEMY = 2;
        public const int PRIORITY_SETUP_ADD_SCORE_SYSTEM = 3;
    }

}
