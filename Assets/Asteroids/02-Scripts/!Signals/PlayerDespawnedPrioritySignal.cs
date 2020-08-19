namespace Asteroid
{
    using HandyPackage;

    public class PlayerDespawnedPrioritySignal : PrioritySignal<PlayerShipComponent>
    {
        public static class Priority
        {
            public const int UNLISTEN_PLAYER_INPUT = 1;
            public const int REMOVE_FROM_POSITION_WRAPPER = 2;
            public const int DECREASE_PLAYER_LIFE = 3;
            public const int RESPAWN_PLAYER = 4;
            public const int ENEMY_FSM = 5;
        }
    }

}
