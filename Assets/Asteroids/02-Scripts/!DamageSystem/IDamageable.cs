namespace Asteroid
{
    public interface IDamageable<TDamagerSource>
    {
        void Damage(int damage, TDamagerSource damager);
    }

}
