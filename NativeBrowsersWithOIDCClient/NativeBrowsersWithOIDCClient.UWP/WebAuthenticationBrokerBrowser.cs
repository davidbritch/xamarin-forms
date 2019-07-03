using System;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;

namespace NativeBrowsersWithOIDCClient.UWP
{
    public class WebAuthenticationBrokerBrowser
    {
        public async Task<string> InvokeAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("Missing url", nameof(url));
            }

            WebAuthenticationResult result;
            try
            {
                result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, new Uri(url));
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            if (result.ResponseStatus == WebAuthenticationStatus.Success)
            {
                return result.ResponseData;
            }
            else if (result.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
            {
                return result.ResponseErrorDetail.ToString();
            }
            else if (result.ResponseStatus == WebAuthenticationStatus.UserCancel)
            {
                return "User cancelled";
            }
            else
            {
                return "Invalid response from WebAuthenticationBroker";
            }
        }
    }

}
