namespace Asteroid
{
    using HandyPackage;

    public class PlayerDespawnedPrioritySignal : PrioritySignal<PlayerRocketController>
    {
        public static class Priority
        {
            public const int UNLISTEN_PLAYER_MOVE_INPUT = 1;
            public const int UNLISTEN_PLAYER_SHOOT_INPUT = 2;
            public const int REMOVE_FROM_POSITION_WRAPPER = 3;
            public const int DECREASE_PLAYER_LIFE = 4;
            public const int RESPAWN_PLAYER = 5;
        }
    }

}
