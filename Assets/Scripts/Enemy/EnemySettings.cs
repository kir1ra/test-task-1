using System;
using UnityEngine;

namespace Test.Enemy
{
    [Serializable]
    [CreateAssetMenu(menuName = "Settings/Enemy Preset")]
    public class EnemySettings : ScriptableObject
    {
        public GameObject Prefab;
        public EnemyModel.Settings Data;
        public IEnemyState.Settings Attack;
    }
}