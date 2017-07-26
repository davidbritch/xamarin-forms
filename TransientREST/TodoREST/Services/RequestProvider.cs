using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TodoREST
{
    public class RequestProvider : IRequestProvider
    {
        readonly HttpClient client;

        public RequestProvider()
        {
			var authData = string.Format("{0}:{1}", Constants.Username, Constants.Password);
			var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

			client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<TResult> GetAsync<TResult>(string uri)
        {
			var response = await client.GetAsync(uri);
			response.EnsureSuccessStatusCode();
            string serialized = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResult>(serialized);
		}

		public async Task<bool> PostAsync<TResult>(string uri, TResult data)
		{
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(uri, content);
			response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
		}

        public async Task<bool> PutAsync<TResult>(string uri, TResult data)
        {
			var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
			var response = await client.PutAsync(uri, content);
			response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
 		}

		public async Task<bool> DeleteAsync(string uri)
        {
            var response = await client.DeleteAsync(uri);
            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }
    }
}
