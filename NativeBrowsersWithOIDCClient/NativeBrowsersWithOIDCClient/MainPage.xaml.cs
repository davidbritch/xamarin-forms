using System;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using IdentityModel.OidcClient;

namespace NativeBrowsersWithOIDCClient
{
    public partial class MainPage : ContentPage
    {
        HttpClient _client;
        string _accessToken;

        public MainPage()
        {
            InitializeComponent();

            _client = new HttpClient();
            _client.BaseAddress = new Uri(Constants.ApiUri);
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var options = new OidcClientOptions
            {
                Authority = Constants.AuthorityUri,
                ClientId = Constants.ClientId,
                Scope = Constants.Scope,
                RedirectUri = Constants.RedirectUri,
                ResponseMode = OidcClientOptions.AuthorizeResponseMode.Redirect
            };

            var oidcClient = new OidcClient(options);
            var state = await oidcClient.PrepareLoginAsync();

            var response = await DependencyService.Get<INativeBrowser>().LaunchBrowserAsync(state.StartUrl);
            // HACK: Replace the RedirectURI, purely for UWP, with the current application callback URI.
            state.RedirectUri = Constants.RedirectUri;
            var result = await oidcClient.ProcessResponseAsync(response, state);

            if (result.IsError)
            {
                Debug.WriteLine("\tERROR: {0}", result.Error);
                return;
            }

            _accessToken = result.AccessToken;
        }

        async void OnCallAPIButtonClicked(object sender, EventArgs e)
        {
            _client.SetBearerToken(_accessToken);

            var response = await _client.GetAsync("test");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine("\tERROR: {0}", response.ReasonPhrase + "\n\t" + content);
                return;
            }

            _editor.Text = JArray.Parse(content).ToString();
        }
    }
}
