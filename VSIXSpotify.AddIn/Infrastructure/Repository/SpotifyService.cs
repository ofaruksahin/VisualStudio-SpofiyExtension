using Autofac;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;
using VSIXSpotify.AddIn.Core.IRepository;
using VSIXSpotify.AddIn.Core.Spotify;

namespace VSIXSpotify.AddIn.Infrastructure.Repository
{
    public class SpotifyService : ISpotifyService
    {
        IAuthService authService;

        public SpotifyService()
        {
            ContainerHelper
                .Build()
                .TryResolve<IAuthService>(out authService);
        }
       

        public async Task<DeviceList> GetDevices()
        {
            var response = await SpotifyClient.Get("/v1/me/player/devices");
            if(response != null && response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<DeviceList>(content);
            }
            return null;
        }

        public async Task<CurrentPlaybackState> GetCurrentPlaybackState()
        {
            var response = await SpotifyClient.Get("/v1/me/player?market=ES");
            if(response != null && response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CurrentPlaybackState>(content);
            }
            return null;
        }
    }
}
