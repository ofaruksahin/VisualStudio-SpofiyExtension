using System.Threading.Tasks;
using VSIXSpotify.AddIn.Core.Spotify;

namespace VSIXSpotify.AddIn.Core.IRepository
{
    public interface ISpotifyService
    {
        Task<DeviceList> GetDevices();

        Task<CurrentPlaybackState> GetCurrentPlaybackState();
        Task<bool> NextSong(Device device);
        Task<bool> PreviousSong(Device device);
        Task<bool> PlayOrPause(CurrentPlaybackState playbackState, Device selectedDevice);
    }
}
