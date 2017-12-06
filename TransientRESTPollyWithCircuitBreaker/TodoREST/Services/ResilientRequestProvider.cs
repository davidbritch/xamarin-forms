using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Polly.CircuitBreaker;
using Polly.Wrap;

namespace TodoREST
{
    public class ResilientRequestProvider : IRequestProvider
    {
        readonly HttpClient client;
        PolicyWrap policyWrap;

        public ResilientRequestProvider()
        {
            var authData = string.Format("{0}:{1}", Constants.Username, Constants.Password);
            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            InitializePolly();
        }

        void InitializePolly()
        {
            var retryPolicy = Policy
                // Don't retry if circuit breaker has broken the circuit
                .Handle<Exception>(e => !(e is BrokenCircuitException))
                .WaitAndRetryAsync(3,
                                   retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                   (exception, delay, retryCount, context) =>
                                   {
                                       Debug.WriteLine($"Retry {retryCount} after {delay.Seconds} seconds delay due to {exception.Message}");
                                   });

            var circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(4,
                                     TimeSpan.FromSeconds(5),
                                     // onBreak
                                     (exception, delay) => Debug.WriteLine($"Breaking the circuit for {delay.Seconds} seconds due to {exception.Message}"),
                                     // onReset
                                     () => Debug.WriteLine($"Call ok - closing the circuit again."),
                                     // onHalfOpen
                                     () => Debug.WriteLine($"Circuit is half-open. The next call is a trial."));

            policyWrap = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
        }

        async Task<HttpResponseMessage> HttpInvoker(Func<Task<HttpResponseMessage>> operation)
        {
            return await policyWrap.ExecuteAsync(operation);
        }

        public async Task<TResult> GetAsync<TResult>(string uri)
        {
            string serialized = null;
            var httpResponse = await HttpInvoker(async () =>
            {
                var response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                serialized = await response.Content.ReadAsStringAsync();
                return response;
            });
            return JsonConvert.DeserializeObject<TResult>(serialized);
        }

        public async Task<bool> PostAsync<TResult>(string uri, TResult data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var httpResponse = await HttpInvoker(async () =>
            {
                var response = await client.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();
                return response;
            });
            return httpResponse.IsSuccessStatusCode;
        }

        public async Task<bool> PutAsync<TResult>(string uri, TResult data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var httpResponse = await HttpInvoker(async () =>
            {
                var response = await client.PutAsync(uri, content);
                response.EnsureSuccessStatusCode();
                return response;
            });
            return httpResponse.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string uri)
        {
            var httpResponse = await HttpInvoker(async () =>
            {
                var response = await client.DeleteAsync(uri);
                response.EnsureSuccessStatusCode();
                return response;
            });
            return httpResponse.IsSuccessStatusCode;
        }
    }
}
