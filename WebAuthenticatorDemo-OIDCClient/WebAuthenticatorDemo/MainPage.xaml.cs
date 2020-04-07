using System;
using System.Net.Http;
using System.Net.Http.Headers;
using IdentityModel.OidcClient;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace WebAuthenticatorDemo
{
    public partial class MainPage : ContentPage
    {
        OidcClient _oidcClient;
        LoginResult _loginResult;
        HttpClient _httpClient;

        public MainPage()
        {
            InitializeComponent();

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(Constants.ApiUri);
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var options = new OidcClientOptions
            {
                Authority = Constants.AuthorityUri,
                ClientId = Constants.ClientId,
                Scope = Constants.Scope,
                RedirectUri = Constants.RedirectUri,
                ResponseMode = OidcClientOptions.AuthorizeResponseMode.Redirect,
                Browser = new Browser()
            };

            _oidcClient = new OidcClient(options);
            _loginResult = await _oidcClient.LoginAsync(new LoginRequest());
            if (_loginResult.IsError)
            {
                Console.WriteLine("ERROR: {0}", _loginResult.Error);
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _loginResult?.AccessToken ?? string.Empty);
        }
        
        async void OnCallAPIButtonClicked(object sender, EventArgs e)
        {
            HttpResponseMessage response = await _httpClient.GetAsync("test");
            string content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                editor.Text = JArray.Parse(content).ToString();
            }
            else
            {
                editor.Text = response.ReasonPhrase;
            }
        }
    }
}
