using System;
using UnityEngine;

namespace Test.Player.Weapon.Projectile
{
    [Serializable]
    [CreateAssetMenu(menuName = "Settings/Bolt Settings")]
    public class BoltSettings : ProjectileSettings
    {
        [Range(0, 1f)]
        public float BounceAimbotFactor;
    }
}