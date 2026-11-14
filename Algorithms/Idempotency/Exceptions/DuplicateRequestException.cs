using System;

public class DuplicateRequestException : IdempotencyException
{
        public DuplicateRequestException(string message, string requestId) 
        : base(message)
    {
        RequestId = requestId;
        ExceptionType = "DuplicateRequest";
    }
}
