public sealed class TokenBucketRateLimiter : IDisposable, ITokenBucketRateLimiter
{
    private readonly object _lock = new();
    private readonly int _capacity;
    private readonly int _refillIntervalInSeconds;
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly PeriodicTimer _timer;
    private readonly Task _refillTask;
    private int _tokens;

    public TokenBucketRateLimiter(int capacity, int refillIntervalInSeconds)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity));

        if (refillIntervalInSeconds <= 0)
            throw new ArgumentOutOfRangeException(nameof(refillIntervalInSeconds));

        _capacity = capacity;
        _refillIntervalInSeconds = refillIntervalInSeconds;

        // Bucket starts full
        _tokens = capacity;

        _timer = new PeriodicTimer(TimeSpan.FromSeconds(_refillIntervalInSeconds));
        _refillTask = RefillTokensAsync();
    }

    public bool TryConsume(int tokens = 1)
    {
        if (tokens <= 0)
            throw new ArgumentOutOfRangeException(nameof(tokens));

        lock (_lock)
        {
            if (_tokens < tokens)
            {
                return false;
            }

            _tokens -= tokens;
            return true;
        }
    }

    public int AvailableTokens
    {
        get
        {
            lock (_lock)
            {
                return _tokens;
            }
        }
    }

    private async Task RefillTokensAsync()
    {
        try
        {
            while (await _timer.WaitForNextTickAsync(_cancellationTokenSource.Token))
            {
                lock (_lock)
                {
                    _tokens = _capacity;
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Expected when disposing.
        }
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();

        _timer.Dispose();
        _cancellationTokenSource.Dispose();

        try
        {
            _refillTask.Wait();
        }
        catch (AggregateException ex) when (
            ex.InnerExceptions.All(e => e is OperationCanceledException))
        {
            // Ignore
        }
    }
}