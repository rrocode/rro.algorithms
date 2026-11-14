using System.Security.Cryptography;
using System.Text.Json.Serialization;

public class IdempotencyService<TResponse> where TResponse: class
{
    private Dictionary<string, IdempotencyRecord<TResponse>> _idempotencyCacheMap = new();
    private object _lock = new();
    public TResponse ProcessRequest<TRequest>(string IdempotencyKey, TRequest request, Func<TResponse> action) where TRequest: class
    {
        if (_idempotencyCacheMap.TryGetValue(IdempotencyKey, out var responseFound))
        {
            if (responseFound.IdempotencyStatus == IdempotencyStatus.Completed) return responseFound.CashedResponse;

            if (responseFound.IdempotencyStatus == IdempotencyStatus.Processing)
            {
                throw new IdempotencyException("Error occurred", IdempotencyKey, "Processing");
            }

            if (responseFound.IdempotencyStatus == IdempotencyStatus.Failed)
            {
                throw new IdempotencyException("Error occurred", IdempotencyKey, "Processing Failed");
            }
        }

        try
        {
            byte[] hashedPayload = Converters.ComputeRequestHash(request);
            TResponse responsePayload = action();
            lock(_lock)
            {
                _idempotencyCacheMap.Add(IdempotencyKey, new IdempotencyRecord<TResponse>
                {
                    IdempotencyKey = IdempotencyKey,
                    IdempotencyStatus = IdempotencyStatus.Completed,
                    CashedResponse = responsePayload,
                });
            }

            return responsePayload;
        }
        catch (Exception ex)
        {
            IdempotencyException iex = new IdempotencyException(ex.Message, IdempotencyKey, "Processing Failed");
            lock (_lock)
            {
                _idempotencyCacheMap[IdempotencyKey].CachedException = iex;
                _idempotencyCacheMap[IdempotencyKey].IdempotencyStatus = IdempotencyStatus.Failed;
            }            
            throw iex;
        }
    }
}