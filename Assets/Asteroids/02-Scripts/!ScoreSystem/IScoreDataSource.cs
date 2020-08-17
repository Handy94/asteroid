namespace Asteroid
{
    public interface IScoreDataSource<TKey>
    {
        int GetScore(TKey key);
    }

}
