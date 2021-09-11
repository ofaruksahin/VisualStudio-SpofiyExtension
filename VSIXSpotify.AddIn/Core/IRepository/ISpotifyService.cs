using System.Threading.Tasks;
using VSIXSpotify.AddIn.Core.Spotify;

namespace VSIXSpotify.AddIn.Core.IRepository
{
    public interface ISpotifyService
    {
        Task<DeviceList> GetDevices();

        Task<CurrentPlaybackState> GetCurrentPlaybackState();
    }
}
