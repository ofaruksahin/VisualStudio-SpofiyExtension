using Newtonsoft.Json;

namespace VSIXSpotify.AddIn.Core.Spotify
{
    public class ExternalUrl
    {
        [JsonProperty("spotify")]
        public string Spotify { get; set; }
    }
}
