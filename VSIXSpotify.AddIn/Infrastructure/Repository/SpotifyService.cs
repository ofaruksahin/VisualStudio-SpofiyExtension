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

        public async Task<bool> NextSong(Device device)
        {
            var response = await SpotifyClient.Post($"/v1/me/player/next?device_id={device.Id}");
            return response != null && response.StatusCode == HttpStatusCode.NoContent;            
        }

        public async Task<bool> PreviousSong(Device device)
        {
            var response = await SpotifyClient.Post($"/v1/me/player/previous?device_id={device.Id}");
            return response != null && response.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<bool> PlayOrPause(CurrentPlaybackState playbackState, Device selectedDevice)
        {
            var result = false;
            if (playbackState.IsPlaying)
            {
                var response = await SpotifyClient.Put($"/v1/me/player/pause?device_id={selectedDevice.Id}");
                result = response != null && response.StatusCode == HttpStatusCode.NoContent;
            }
            else
            {
                var response = await SpotifyClient.Put($"/v1/me/player/play?device_id={selectedDevice.Id}", new { });
                result = response != null && response.StatusCode == HttpStatusCode.NoContent;
            }
            return result;
        }
    }
}
