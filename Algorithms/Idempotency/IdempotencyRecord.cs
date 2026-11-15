public class IdempotencyRecord<TResponse> where TResponse : class
{
    public string? IdempotencyKey { get; init; }
    public IdempotencyStatus IdempotencyStatus { get; set; }
    public string? PayloadHash { get; init; }
    public TResponse? CashedResponse { get; set; }
    public IdempotencyException? CachedException { get; set; }
}