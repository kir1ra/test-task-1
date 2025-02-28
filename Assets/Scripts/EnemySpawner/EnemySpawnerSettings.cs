using System;
using UnityEngine;

namespace Test.Enemy
{
    [Serializable]
    [CreateAssetMenu(menuName = "Settings/EnemySpawner Settings")]
    public class EnemySpawnerSettings : ScriptableObject
    {
        public int StartEnemyCount;
        public int MaxEnemyCount;

        [Range(0.1f, 5f)]
        public float EnemySpawnRate = 0.5f;
        //public float EnemySpawnAmountIncrement = 0.2f;

        public float EnemySpawnMargin = 1;

        public float RespawnCheckInterval = 1f;
        public float RespawnMargin = 2;

        public EnemySettings EnemyPreset;
    }
}