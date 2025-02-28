using System;
using UnityEngine;

namespace Test.Enemy
{
    public interface IEnemyState
    {
        void EnterState();
        void ExitState();
        void ExecuteUpdate();



        [Serializable]
        public class Settings
        {
            public float AttackRange = 1.0f;
            [Min(float.Epsilon)]
            public float AttackSpeed = 2.0f;
            public int AttackDamage = 5;
        }
    }
}