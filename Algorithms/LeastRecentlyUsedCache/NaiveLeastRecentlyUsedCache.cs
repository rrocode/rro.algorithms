using Algorithms.LeastRecentlyUsedCache;
public sealed class NaiveLeastRecentlyUsedCache<TKey, TValue> : ILeastRecentlyUsedCache<TKey, TValue> where TKey : notnull
{
    private readonly int _capacity;
    private readonly Dictionary<TKey, LinkedListNode<CacheItem>> _cacheMap;
    private readonly LinkedList<CacheItem> _lruList;

    public NaiveLeastRecentlyUsedCache(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than 0.");
        }

        _capacity = capacity;
        _cacheMap = new Dictionary<TKey, LinkedListNode<CacheItem>>(capacity);
        _lruList = new LinkedList<CacheItem>();
    }

    public void Add(TKey key, TValue value)
    {
        if (_cacheMap.TryGetValue(key, out var existingNode))
        {

            existingNode.Value = new CacheItem(key, value);
            _lruList.Remove(existingNode);
            _lruList.AddFirst(existingNode);
            
            return;
        }

        if (_cacheMap.Count >= _capacity)
        {
            var lruNode = _lruList.Last;
            if (lruNode != null)
            {
                _cacheMap.Remove(lruNode.Value.Key);
                _lruList.RemoveLast();
            }
        }

        var newNode = new LinkedListNode<CacheItem>(new CacheItem(key, value));
        _lruList.AddFirst(newNode);
        _cacheMap[key] = newNode;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        if (!_cacheMap.TryGetValue(key, out var node))
        {
            value = default!;
            return false;
        }

        _lruList.Remove(node);
        _lruList.AddFirst(node);

        value = node.Value.Value;
        return true;
    }

    private record CacheItem(TKey Key, TValue Value);
}