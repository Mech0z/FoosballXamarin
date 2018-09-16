using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FoosballXamarin.Services
{
    public class HttpClientWrapper : HttpClient
    {
        public new async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            try
            {
                var httpResponseMessage = await SendAsync(request, HttpCompletionOption.ResponseContentRead, CancellationToken.None);
                if ((int)httpResponseMessage.StatusCode == 419)
                {
                    MessagingCenter.Send(this, "TokenExpired");
                    Preferences.Remove("UserSettings");
                }
                return httpResponseMessage;
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