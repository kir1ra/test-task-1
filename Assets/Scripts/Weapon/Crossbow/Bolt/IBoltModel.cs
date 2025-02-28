using UnityEngine;

namespace Test.Player.Weapon.Projectile
{
    public interface IBoltModel : IProjectileModel
    {
        Vector2 Direction { get; set; }
        float BounceAimbotFactor { get; }
    }
}