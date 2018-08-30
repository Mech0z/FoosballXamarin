using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FoosballXamarin.Services
{
    public class HttpClientWrapper : HttpClient
    {
        public new async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            try
            {
                return await SendAsync(request, HttpCompletionOption.ResponseContentRead, CancellationToken.None);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("An invalid request URI "))
                {
                    //TODO Show error and make user choose to either try again, close app and wait or enter new API url
                    Preferences.Remove("ApiUrlSettings");
                    return new HttpResponseMessage(HttpStatusCode.BadGateway);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}