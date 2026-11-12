namespace Algorithms.CircuitBreaker;
public readonly record struct RequestMessage(DateTime Timestamp, bool isSuccess);