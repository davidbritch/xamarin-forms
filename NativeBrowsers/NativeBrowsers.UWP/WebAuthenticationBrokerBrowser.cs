using System;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using NativeBrowsers.Models;

namespace NativeBrowsers.UWP
{
    public class WebAuthenticationBrokerBrowser
    {
        public async Task<BrowserResult> InvokeAsync(string url)
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
                return new BrowserResult
                {
                    ResultType = BrowserResultType.UnknownError,
                    Error = ex.ToString()
                };
            }

            if (result.ResponseStatus == WebAuthenticationStatus.Success)
            {
                return new BrowserResult
                {
                    ResultType = BrowserResultType.Success,
                    Response = result.ResponseData
                };
            }
            else if (result.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
            {
                return new BrowserResult
                {
                    ResultType = BrowserResultType.HttpError,
                    Error = string.Concat(result.ResponseErrorDetail.ToString())
                };
            }
            else if (result.ResponseStatus == WebAuthenticationStatus.UserCancel)
            {
                return new BrowserResult
                {
                    ResultType = BrowserResultType.UserCancel
                };
            }
            else
            {
                return new BrowserResult
                {
                    ResultType = BrowserResultType.UnknownError,
                    Error = "Invalid response from WebAuthenticationBroker"
                };
            }
        }
    }
}
