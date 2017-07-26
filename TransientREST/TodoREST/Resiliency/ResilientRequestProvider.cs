using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TodoREST
{
    public class ResilientRequestProvider : IRequestProvider
	{
		readonly HttpClient client;
        readonly IRetry retry;

        public ResilientRequestProvider(IRetry resilience)
		{
            retry = resilience;

			var authData = string.Format("{0}:{1}", Constants.Username, Constants.Password);
			var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

			client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		async Task<HttpResponseMessage> HttpInvoker(Func<Task<HttpResponseMessage>> operation)
		{
            return await retry.RetryOnExceptionAsync<HttpRequestException>(operation);
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
