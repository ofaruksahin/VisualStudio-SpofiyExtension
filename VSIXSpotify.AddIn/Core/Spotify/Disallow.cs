using Newtonsoft.Json;

namespace VSIXSpotify.AddIn.Core.Spotify
{
    public class Disallow
    {
        [JsonProperty("pausing")]
        public bool Pausing { get; set; }
    }
}
