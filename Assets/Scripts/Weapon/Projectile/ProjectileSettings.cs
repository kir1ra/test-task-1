using System;
using UnityEngine;

namespace Test.Player.Weapon.Projectile
{
    [Serializable]
    public abstract class ProjectileSettings : ScriptableObject
    {
        public GameObject Prefab;
     
        [Range(0, 15)]
        public float Speed;

        [Range(float.Epsilon, 5)]
        public float Lifetime;

        [Range(0, 100)]
        public int Damage;
    }
}