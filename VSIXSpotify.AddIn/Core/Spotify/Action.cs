using Newtonsoft.Json;

namespace VSIXSpotify.AddIn.Core.Spotify
{
    public class Action
    {
        [JsonProperty("disallows")]
        public Disallow Disallow { get; set; }
    }
}
