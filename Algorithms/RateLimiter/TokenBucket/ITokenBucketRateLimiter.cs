public interface ITokenBucketRateLimiter
{
    bool TryConsume(int tokens);
    int AvailableTokens { get; }
}