using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace TodoREST
{
    public sealed class RetryWithExponentialBackoff : IRetry
    {
        readonly int maxRetries;
        readonly int delayMilliseconds;
        readonly int maxDelayMilliseconds;

        public RetryWithExponentialBackoff(int retries = 10, int delay = 200, int maxDelay = 2000)
        {
            maxRetries = retries;
            delayMilliseconds = delay;
            maxDelayMilliseconds = maxDelay;
        }

        public async Task<HttpResponseMessage> RetryOnExceptionAsync<TException>(Func<Task<HttpResponseMessage>> operation) where TException : Exception
        {
            HttpResponseMessage response;
            var backoff = new ExponentialBackoff(maxRetries, delayMilliseconds, maxDelayMilliseconds);
            while (true)
            {
                try
                {
                    response = await operation();
                    break;
                }
                catch (Exception ex) when (ex is TimeoutException ||
                                           ex is TException)
                {
                    Debug.WriteLine("Exception: " + ex.Message);
                    await backoff.Delay();
                }
            }
            return response;
        }
    }
}
