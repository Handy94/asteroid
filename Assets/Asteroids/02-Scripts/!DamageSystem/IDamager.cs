namespace Asteroid
{
    public interface IDamager<TDamagerType>
    {
        TDamagerType DamagerType { get; }
        int GetDamage();
    }

}
