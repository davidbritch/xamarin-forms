using System;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using IdentityModel.Client;
using NativeBrowsers.Extensions;
using NativeBrowsers.Services;

namespace NativeBrowsers
{
    public partial class MainPage : ContentPage
    {
        HttpClient _client;
        IIdentityService _identityService;
        IDependencyService _dependencyService;
        AuthorizeResponse _authResponse;

        public MainPage()
        {
            InitializeComponent();

            _client = new HttpClient();
            _client.BaseAddress = new Uri(Constants.ApiUri);

            _identityService = new IdentityService(new RequestProvider());
            _dependencyService = new NativeBrowsers.Services.DependencyService();
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            string url = _identityService.CreateAuthorizationRequest();
            var result = await _dependencyService.Get<INativeBrowser>().LaunchBrowserAsync(url);

            if (!result.IsError)
            {
                _authResponse = new AuthorizeResponse(result.Response);
                if (_authResponse.IsError)
                {
                    Debug.WriteLine("\tERROR: {0}", _authResponse.Error);
                }
            }
        }

        async void OnCallAPIButtonClicked(object sender, EventArgs e)
        {
            if (!_authResponse.IsError && _authResponse.Code.IsPresent())
            {
                var userToken = await _identityService.GetTokenAsync(_authResponse.Code);
                if (userToken.AccessToken.IsPresent())
                {
                    _client.SetBearerToken(userToken.AccessToken);

                    var result = await _client.GetAsync("test");
                    var content = await result.Content.ReadAsStringAsync();
                    if (!result.IsSuccessStatusCode)
                    {
                        Debug.WriteLine("\tERROR: {0}", result.ReasonPhrase + "\n\t" + content);
                        return;
                    }

                    _editor.Text = JArray.Parse(content).ToString();
                }
            }
        }
    }
}
