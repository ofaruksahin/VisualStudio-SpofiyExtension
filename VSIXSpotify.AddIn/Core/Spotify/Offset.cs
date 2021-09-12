using Newtonsoft.Json;

namespace VSIXSpotify.AddIn.Core.Spotify
{
    public class Offset
    {
        [JsonProperty("position")]
        public int Position { get; set; }
    }
}
