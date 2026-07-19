using Xunit;

namespace Algorithms.Tests.RateLimiter.TokenBucket;

public class TokenBucketRateLimiterTests
{
    public static IEnumerable<object[]> TestCases =>
    [
        [1, true],
        [5, true],
        [10, true],
        [11, false],
        [15, false],
    ];

    [Theory]
    [MemberData(nameof(TestCases))]
    public void TryConsume(int tokens, bool expected)
    {
        var sut = new TokenBucketRateLimiter(10, 5); // 10 tokens per 5 seconds
        Assert.Equal(expected, sut.TryConsume(tokens));
    }

    [Fact]
    public void TryConsume_Throws_ForNonPositiveTokenCount()
    {
        var sut = new TokenBucketRateLimiter(10, 5);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => sut.TryConsume(0));

        Assert.Equal("tokens", exception.ParamName);

        var exception2 = Assert.Throws<ArgumentOutOfRangeException>(() => sut.TryConsume(-1));

        Assert.Equal("tokens", exception.ParamName);
    }

    [Fact]
    public void TryConsume_DecrementAvailableTokens()
    {
        var sut = new TokenBucketRateLimiter(10, 5);

        Assert.True(sut.TryConsume(5));
        Assert.True(sut.TryConsume(5));
        Assert.False(sut.TryConsume(1));
    }

    [Fact]
    public async Task Bucket_IsRefilled_AfterInterval()
    {
        var sut = new TokenBucketRateLimiter(10, 1);

        Assert.True(sut.TryConsume(10));
        Assert.False(sut.TryConsume(1));

        await Task.Delay(1100);

        Assert.True(sut.TryConsume(10));
    }

    [Fact]
    public async Task ConcurrentRequests_DoNotExceedCapacity()
    {
        var sut = new TokenBucketRateLimiter(100, 60);

        var tasks = Enumerable.Range(0, 200)
            .Select(_ => Task.Run(() => sut.TryConsume()));

        var results = await Task.WhenAll(tasks);

        Assert.Equal(100, results.Count(r => r));
    }
}