using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace Test.Player.Weapon
{
    public enum WeaponType
    {
        Crossbow
    }

    public abstract class WeaponPresenter
    {
        protected readonly IWeaponModel _weaponModel;
        protected readonly PlayerPresenter _player;
        protected readonly CompositeDisposable _disposer;

        IReadOnlyReactiveProperty<float> _playerAttackSpeed;

        public WeaponPresenter(
            PlayerPresenter player, 
            CompositeDisposable disposer, 
            IWeaponModel weaponModel)
        {
            _player = player;
            _disposer = disposer;
            _weaponModel = weaponModel;
        }


        /// <summary>
        /// We use manual [Inject] because IInitializable force 
        /// Zenject to instantiate the class at startup despite being in a Factory
        /// </summary>
        [Inject]
        public virtual void Initialize()
        {
            _playerAttackSpeed = _player.AttackSpeed.ToReactiveProperty();

            _weaponModel.TimeUntilNextFire.Where(t => t <= 0)
                                          .Subscribe(_ => FireInternal())
                                          .AddTo(_disposer);

        }
        
        public void Update()
        {
            _weaponModel.TimeUntilNextFire.Value -=  _playerAttackSpeed.Value * Time.deltaTime;
        }

        private void FireInternal()
        {
            _weaponModel.TimeUntilNextFire.Value += _weaponModel.Cooldown;
            Fire().Forget();
        }

        protected abstract UniTaskVoid Fire();

    }
}
