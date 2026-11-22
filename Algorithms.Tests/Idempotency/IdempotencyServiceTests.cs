using Xunit;
using Algorithms.Idempotency;

namespace Algorithms.Tests.Idempotency;

public class TestResponse
{
    public int Id { get; set; }
    public bool Success { get; set; }
}

public class TestRequest
{
    public string? IdempotencyKey { get; init; }
    public double PaymentAmount { get; init; }
    public string? Currency { get; init; }
    public DateTime ValueDate { get; init; }
    public string? InvoiceNumber { get; init; }
}

public class IdempotencyServiceTests
{
    [Fact]
    public void Assert_AddNewIdempotencyRecordWhenDoesntExist()
    {
        var sut = new IdempotencyService<TestResponse>();

        var req = new TestRequest
        {
            IdempotencyKey = "KEY-1",
            PaymentAmount = 11.98,
            Currency = "EUR",
            ValueDate = DateTime.Today,
            InvoiceNumber = $"{DateTime.Today}-EUR-11c98"
        };

        

        try
        {
            var response = sut.ProcessRequest<TestRequest>("KEY-1", req, this.getActionByType(true));
            Assert.True(response.Success == true);
            Assert.True(response.Id == 1);
        } catch (IdempotencyException iex)
        {
        }
    }

    [Fact]
    public void Assert_AddNewIdempotencyRecordWithExceptionWhenCallFails()
    {
        var sut = new IdempotencyService<TestResponse>();

        var req = new TestRequest
        {
            IdempotencyKey = "KEY-2",
            PaymentAmount = 11.98,
            Currency = "EUR",
            ValueDate = DateTime.Today,
            InvoiceNumber = $"{DateTime.Today}-EUR-11c98"
        };

        

        try
        {
            var response = sut.ProcessRequest<TestRequest>("KEY-1", req, this.getActionByType(false));
            Assert.True(response.Success == false);
            Assert.True(response.Id == 1);
        } catch (IdempotencyException iex)
        {
        }
    }

    private Func<TestResponse> getActionByType(bool success)
    {

        Func<TestResponse> actionSuccess = () => {
            Task.Delay(1000);
            return new TestResponse {
                Id = 1,
                Success = true
            };
        };

        Func<TestResponse> actionError = () => {
            Task.Delay(3000);
            return new TestResponse {
                Id = 1,
                Success = false
            };
        };

        return success ? actionSuccess : actionError;
    }
}