namespace Test.Player.Weapon.Projectile
{
    public abstract class ProjectileModel : IProjectileModel
    {
        protected readonly ProjectileSettings _settings;

        protected ProjectileModel(ProjectileSettings settings)
        {
            _settings = settings;
        }

        float IProjectileModel.Speed => _settings.Speed;
        float IProjectileModel.Lifetime => _settings.Lifetime;
        int IProjectileModel.Damage => _settings.Damage;
    }
}