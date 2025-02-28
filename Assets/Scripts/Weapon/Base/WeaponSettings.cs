using System;
using Test.Player.Weapon.Projectile;
using UnityEngine;

namespace Test.Player.Weapon
{
    [Serializable]
    public abstract class WeaponSettings : ScriptableObject
    {
        [Range(.2f, 5)]
        public float Cooldown;
        [Range(1, 5)]
        public int BurstCount;
        [Range(0, 1)]
        public float BurstInterval;

        public ProjectileSettings ProjectileSettings;
    }
}