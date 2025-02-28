using UnityEngine;

namespace Test.Player.Weapon.Projectile
{
    public class BoltModel : ProjectileModel, IBoltModel
    {
        protected readonly BoltSettings _boltSettings;

        public BoltModel(BoltSettings boltSettings) : base(boltSettings)
        {
            _boltSettings = boltSettings;
        }

        Vector2 IBoltModel.Direction { get; set; }

        float IBoltModel.BounceAimbotFactor => _boltSettings.BounceAimbotFactor;

    }
}
