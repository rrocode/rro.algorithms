public interface ICircuitBreaker<T>
{
    T Execute(Func<T> func);
}

public interface ICircuitBreakerAsync<T>
{
    Task<T> ExecuteAsync(Func<Task<T>> func);
}