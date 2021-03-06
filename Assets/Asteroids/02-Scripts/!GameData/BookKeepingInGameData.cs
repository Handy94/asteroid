﻿using UniRx;

namespace Asteroid
{
    public class BookKeepingInGameData
    {
        public IntReactiveProperty PlayerLife = new IntReactiveProperty(3);
        public IntReactiveProperty Score = new IntReactiveProperty(0);
        public IntReactiveProperty HighScore = new IntReactiveProperty(0);

        public IntReactiveProperty CurrentStage = new IntReactiveProperty(1);
        public IntReactiveProperty AsteroidCount = new IntReactiveProperty(0);

        public PlayerShipComponent PlayerShipComponent;
    }

}
