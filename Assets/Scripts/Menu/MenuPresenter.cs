using Cysharp.Threading.Tasks;
using UniRx;
using Zenject;

namespace Test.Menu
{
    public class MenuPresenter : IInitializable
    {
        readonly MenuView _menuView;
        readonly GameController _gameController;

        public MenuPresenter(MenuView menuView,
                             GameController gameController)
        {
            _menuView = menuView;
            _gameController = gameController;
        }

        void IInitializable.Initialize()
        {
            _menuView.OnNewGameRequested.Subscribe(_ => OnNewGameRequested().Forget());
            _menuView.OnReviveAndContinueRequested.Subscribe(_ => OnReviveAndContinueRequested().Forget());

            _gameController.State.Where(gameState => gameState == GameStates.GameOver)
                                 .Subscribe(_ => _menuView.ShowGameOverPanel().Forget());
        }

        public async UniTaskVoid OnNewGameRequested()
        {
            await _menuView.HideStartButton();
            _gameController.StartNewGame();
        }
        public async UniTaskVoid OnReviveAndContinueRequested()
        {
            await _menuView.HideGameOverPanel();
            _gameController.ReviveAndResume().Forget();
        }
    }
}