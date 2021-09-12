using Newtonsoft.Json;

namespace VSIXSpotify.AddIn.Core.Spotify
{
    public class Play
    {
        [JsonProperty("context_uri")]
        public string ContextUri { get; set; }

        [JsonProperty("offset")]
        public Offset Offset { get; set; }

        [JsonProperty("position_ms")]
        public int PositionMs { get; set; }

        public Play()
        {
            Offset = new Offset();
        }
    }
}
