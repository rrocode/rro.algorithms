namespace Algorithms.LeastRecentlyUsedCache;
public interface ILeastRecentlyUsedCache<TKey, TValue>
{
    void Add(TKey key, TValue value);
    bool TryGetValue(TKey key, out TValue value);
}