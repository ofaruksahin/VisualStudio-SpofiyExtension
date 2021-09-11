using Newtonsoft.Json;

namespace VSIXSpotify.AddIn.Core.Spotify
{
    public class ExternalId
    {
        [JsonProperty("isrc")]
        public string Isrc { get; set; }
    }
}
