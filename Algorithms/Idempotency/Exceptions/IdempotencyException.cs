public class IdempotencyException : Exception
{
    public string? RequestId { get; set; }
    public string? ExceptionType { get; set; } 
    public IdempotencyException() 
    { 
    }
    public IdempotencyException(string message) 
        : base(message) 
    { 
    }

    public IdempotencyException(string message, Exception innerException) 
        : base(message, innerException) 
    { 
    }

    // Optional: Custom constructor(s) to populate your extra properties
    public IdempotencyException(string message, string requestId, string exectionType) 
        : base(message)
    {
        RequestId = requestId;
        ExceptionType = exectionType;
    }
}