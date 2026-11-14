using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

public static class Converters
{
    public static byte[] GetHashBytes(string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        return System.Security.Cryptography.SHA256.HashData(bytes);
    }

    public static byte[] ComputeRequestHash<TRequest>(TRequest request) where TRequest: class
    {
        byte[] utf8Json = JsonSerializer.SerializeToUtf8Bytes(request);
        byte[] hashBytes = System.Security.Cryptography.SHA256.HashData(utf8Json);

        return hashBytes;
    }
}