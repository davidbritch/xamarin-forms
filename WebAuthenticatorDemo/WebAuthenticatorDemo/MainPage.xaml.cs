using System;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using WebAuthenticatorDemo.Extensions;
using WebAuthenticatorDemo.Models;
using WebAuthenticatorDemo.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WebAuthenticatorDemo
{
    public partial class MainPage : ContentPage
    {
        IIdentityService identityService;
        AuthorizeResponse authorizeResponse;

        public MainPage()
        {
            InitializeComponent();

            identityService = new IdentityService(new RequestProvider());
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            string url = identityService.CreateAuthorizationRequest();
            WebAuthenticatorResult authResult = await WebAuthenticator.AuthenticateAsync(new Uri(url), new Uri(Constants.RedirectUri));

            string raw = ParseAuthenticatorResult(authResult);
            authorizeResponse = new AuthorizeResponse(raw);
            if (authorizeResponse.IsError)
            {
                Console.WriteLine("ERROR: {0}", authorizeResponse.Error);
            }
        }

        string ParseAuthenticatorResult(WebAuthenticatorResult result)
        {
            string code = result?.Properties["code"];
            string idToken = result?.IdToken;
            string scope = result?.Properties["scope"];
            string state = result?.Properties["state"];
            string sessionState = result?.Properties["session_state"];

            return $"{Constants.RedirectUri}#code={code}&id_token={idToken}&scope={scope}&state={state}&session_state={sessionState}";
        }
        
        async void OnCallAPIButtonClicked(object sender, EventArgs e)
        {
            if (!authorizeResponse.IsError && authorizeResponse.Code.IsPresent())
            {
                UserToken userToken = await identityService.GetTokenAsync(authorizeResponse.Code);
                if (userToken.AccessToken.IsPresent())
                {
                    var content = await identityService.GetAsync($"{Constants.ApiUri}test", userToken.AccessToken);
                    editor.Text = JArray.Parse(content).ToString();
                }
            }
        }
    }
}
