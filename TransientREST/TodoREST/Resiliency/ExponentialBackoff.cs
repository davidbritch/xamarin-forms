using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TodoREST
{
    public struct ExponentialBackoff
    {
        readonly int maxRetries, delayMilliseconds, maxDelayMilliseconds;
        int retries, pow;

        public ExponentialBackoff(int noOfRetries, int delay, int maxDelay)
        {
            maxRetries = noOfRetries;
            delayMilliseconds = delay;
            maxDelayMilliseconds = maxDelay;
            retries = 0;
            pow = 1;
        }

        public Task Delay()
        {
            if (retries < maxRetries)
            {
                retries++;
                pow = pow << 1;
            }
            else
            {
                throw new TimeoutException($"{maxRetries} retry attempts made. Retries failed.");
            }

            int delay = Math.Min(delayMilliseconds * (pow - 1) / 2, maxDelayMilliseconds);
            Debug.WriteLine($"Retry {retries} after {delay} milliseconds delay. Maximum delay is {maxDelayMilliseconds} milliseconds.");
            return Task.Delay(delay);
        }
    }
}
