namespace Test.Player.Weapon.Projectile
{
    public interface IProjectileModel
    {
        float Speed { get; }
        float Lifetime { get; }
        int Damage { get; }
    }
}