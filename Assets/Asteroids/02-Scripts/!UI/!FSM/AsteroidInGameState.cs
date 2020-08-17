namespace Asteroid
{
    using HandyPackage;
    using HandyPackage.FSM;
    using ParadoxNotion.Design;

    [Category("@@-Handy Package/States")]
    [Name("Asteroid InGame State")]
    public class AsteroidInGameState : UIBaseState
    {
        private GameStarterSystem _gameStarterSystem;

        protected override void OnInit()
        {
            base.OnInit();

            _gameStarterSystem = DIResolver.GetObject<GameStarterSystem>();
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            _gameStarterSystem.StartGame();
        }
    }

}
