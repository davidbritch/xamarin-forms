using System;
using System.Threading.Tasks;
using System.Net.Http;

namespace TodoREST
{
    public interface ICircuitBreakerService
    {
        string ResourceName { get; }
        bool IsClosed { get; }
        bool IsOpen { get; }

        Task<HttpResponseMessage> InvokeAsync(Func<Task<HttpResponseMessage>> operation);
        Task<HttpResponseMessage> InvokeAsync(Func<Task<HttpResponseMessage>> operation, Action<CircuitBreakerOpenException> circuitBreakerOpenAction, Action<Exception> anyOtherExceptionAction);
    }
}
