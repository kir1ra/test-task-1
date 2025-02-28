using System;
using Test.Enemy;
using Test.Player;
using Test.Player.Weapon;
using UnityEngine;
using Zenject;

namespace Test
{
    [CreateAssetMenu(menuName = "Settings/Game Settings")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        [SerializeField] DebugSettings _debugSettings;
        [SerializeField] PlayerModel.Settings _playerSettings;
        [SerializeField] EnemySpawnerSettings _enemySpawnerSettings;

        [SerializeField] WeaponSettings _weaponSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(_debugSettings).IfNotBound();
            Container.BindInstance(_playerSettings).IfNotBound();

            Container.BindInstance(_enemySpawnerSettings);

            Container.Bind<CrossbowSettings>()
                     .FromInstance(_weaponSettings as CrossbowSettings)
                     .WhenInjectedInto<CrossbowInstaller>();
        }
    }

    [Serializable]
    public class DebugSettings
    {
        public bool EditorDrawLine;
    }
}