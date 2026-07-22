using Algorithms.LeastRecentlyUsedCache;
using Xunit;

namespace Algorithms.Tests.LeastRecentlyUsedCache;

public class LeastRecentlyUsedCacheTests
{
    [Fact]
    public void Add_And_TryGetValue_Works_Correctly()
    {
        var sut = new NaiveLeastRecentlyUsedCache<int, string>(3);

        sut.Add(1, "one");
        sut.Add(2, "two");
        sut.Add(3, "three");

        Assert.True(sut.TryGetValue(1, out var value1));
        Assert.Equal("one", value1);

        Assert.True(sut.TryGetValue(2, out var value2));
        Assert.Equal("two", value2);

        Assert.True(sut.TryGetValue(3, out var value3));
        Assert.Equal("three", value3);
    }

    [Fact]
    public void Add_Remove_LeastRecentlyUsed_Item_When_Capacity_Is_Exceeded()
    {
        var sut = new NaiveLeastRecentlyUsedCache<int, string>(2);

        sut.Add(1, "one");
        sut.Add(2, "two");

        // Access key 1 to make it recently used
        Assert.True(sut.TryGetValue(1, out _));

        // Add a new item, which should evict key 2
        sut.Add(3, "three");

        Assert.False(sut.TryGetValue(2, out _)); // Key 2 should be evicted
        Assert.True(sut.TryGetValue(1, out _));  // Key 1 should still be present
        Assert.True(sut.TryGetValue(3, out _));  // Key 3 should be present
    }
}