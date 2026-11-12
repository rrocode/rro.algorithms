using System.ComponentModel.DataAnnotations;

namespace Algorithms.CircuitBreaker;
public class CircuitBreaker<T> : ICircuitBreaker<T>
{
    private readonly object _lock = new();
    private readonly Queue<RequestMessage> _requestQueue = new();
    private readonly int _maxRequestCount = 30;
    private readonly double _maxFailureTreashold = 0.5;
    private DateTime _openStateEnteredAt;
    private readonly TimeSpan _resetInterval = TimeSpan.FromSeconds(30);
    private readonly TimeSpan _windowDuration = TimeSpan.FromSeconds(60);
    private CircuitBreakerState _state = CircuitBreakerState.Closed;

    public T Execute(Func<T> action)
    {
        lock ( _lock ) {
            EvaluateStateAndSetToHalfOpenIfIntervalIsReached();

            if (_state == CircuitBreakerState.Open)
            {
                throw new InvalidOperationException($"CircuitBreaker is OPEN, no request processing is allowe till interval is reached");
            }
        }

        bool isSuccess = false;
        try
        {
            T result = action();
            isSuccess = true;
            RecordResults(isSuccess);
            return result;
        } 
        catch(Exception ex)
        {
            RecordResults(false);
            throw;
        }
    }

    private void RecordResults(bool isSuccess)
    {
        lock (_lock) {
            var now = DateTime.UtcNow;

            while (_requestQueue.Count > 0 && (now - _requestQueue.Peek().Timestamp > _windowDuration))
            {
                _requestQueue.Dequeue();
            }

            _requestQueue.Enqueue(new RequestMessage(now, isSuccess));

            if (_state == CircuitBreakerState.Closed)
            {
                if (_requestQueue.Count > _maxRequestCount)
                {
                    int failures = 0;
                    foreach (var request in _requestQueue)
                    {
                        if (!request.isSuccess)
                        {
                            failures++;
                        }
                    }
                    
                    var failureRate = (double)failures / (double)_requestQueue.Count;
                    if (failureRate > _maxFailureTreashold)
                    {
                        setStateToOpen();
                    }
                }
            } else if (_state == CircuitBreakerState.HalfOpen) {
                if (isSuccess)
                {
                    _state = CircuitBreakerState.Closed;
                    _requestQueue.Clear();
                } else
                {
                    setStateToOpen();
                }
            }
        }
    }

    private void setStateToOpen()
    {
        _state = CircuitBreakerState.Open;
        _openStateEnteredAt = DateTime.UtcNow;
    }

    private void EvaluateStateAndSetToHalfOpenIfIntervalIsReached()
    {
        // to be called within a lock
        if (_state == CircuitBreakerState.Open)
        {
            if (_requestQueue.Count > 0 && DateTime.UtcNow - _openStateEnteredAt > _resetInterval)
            {
                _state = CircuitBreakerState.HalfOpen;
                _requestQueue.Clear();    
            }
        }
    }
}    
