using Newtonsoft.Json;
using System.Collections.Generic;

namespace VSIXSpotify.AddIn.Core.Spotify
{
    public class DeviceList
    {
        [JsonProperty("devices")]
        public List<Device> Devices { get; set; }
    }
}
