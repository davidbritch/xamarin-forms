using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TodoREST
{
    public interface IRetry
    {
        Task<HttpResponseMessage> RetryOnExceptionAsync<TException>(Func<Task<HttpResponseMessage>> operation) where TException : Exception;
    }
}