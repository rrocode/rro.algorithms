using System.Security.Cryptography;
using System.Text.Json.Serialization;

public class IdempotencyService<TResponse> where TResponse: class
{
    private Dictionary<string, IdempotencyRecord<TResponse>> _idempotencyCacheMap = new();
    private object _lock = new();
    public TResponse ProcessRequest<TRequest>(string IdempotencyKey, TRequest request, Func<TResponse> action) where TRequest: class
    {
        if (_idempotencyCacheMap.TryGetValue(IdempotencyKey, out var recordFound))
        {
            if (recordFound.PayloadHash == Converters.ComputeHash(request))
            {
                if (recordFound.IdempotencyStatus == IdempotencyStatus.Completed) return recordFound.CashedResponse;

                if (recordFound.IdempotencyStatus == IdempotencyStatus.Processing)
                {
                    throw new IdempotencyException("Error occurred", IdempotencyKey, "Processing");
                }

                if (recordFound.IdempotencyStatus == IdempotencyStatus.Failed)
                {
                    throw new IdempotencyException("Error occurred", IdempotencyKey, "Processing Failed");
                }
            } else
            {
                throw new IdempotencyException("Error occurred", IdempotencyKey, "Duplicate");
            }
        }

        string hashedPayload = string.Empty;

        try
        {
            hashedPayload = Converters.ComputeHash(request);
            TResponse responsePayload = action();
            lock(_lock)
            {
                _idempotencyCacheMap.Add(IdempotencyKey, new IdempotencyRecord<TResponse>
                {
                    IdempotencyKey = IdempotencyKey,
                    IdempotencyStatus = IdempotencyStatus.Completed,
                    CashedResponse = responsePayload,
                    PayloadHash = hashedPayload
                });
            }

            return responsePayload;
        }
        catch (Exception ex)
        {
            IdempotencyException iex = new IdempotencyException(ex.Message, IdempotencyKey, "Processing Failed");
            lock (_lock)
            {
                var failedIdempotencyRecord = new IdempotencyRecord<TResponse>
                {
                    IdempotencyKey = IdempotencyKey,
                    PayloadHash = hashedPayload,
                    IdempotencyStatus = IdempotencyStatus.Failed,
                    CachedException = iex

                };
                _idempotencyCacheMap.Add(IdempotencyKey, failedIdempotencyRecord);
            }            
            throw iex;
        }
    }
}