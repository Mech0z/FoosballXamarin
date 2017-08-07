using System.Collections.Generic;
using System.Threading.Tasks;
using FoosballXamarin.Services;
using Models;
using Newtonsoft.Json;

[assembly: Xamarin.Forms.Dependency(typeof(UserService))]
namespace FoosballXamarin.Services
{
    public class UserService : BaseService, IUserService
    {
        public async Task<List<User>> GetDataAsync()
        {
            RestUrl = App.ApiUrl + "player/GetUsers";
            var response = await _client.GetAsync(HttpUri);

            if (!response.IsSuccessStatusCode) return new List<User>();

            var content = await response.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<List<User>>(content);
            return items;
        }
    }
}