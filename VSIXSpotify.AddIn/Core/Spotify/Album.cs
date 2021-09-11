using Newtonsoft.Json;
using System.Collections.Generic;

namespace VSIXSpotify.AddIn.Core.Spotify
{
    public class Album
    {
        [JsonProperty("album_type")]
        public string AlbumType { get; set; }
        [JsonProperty("artists")]
        public List<Artist> Artists{ get; set; }
        [JsonProperty("external_urls")]
        public ExternalUrl ExternalUrl { get; set; }
        [JsonProperty("href")]
        public string Href { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("images")]
        public List<Image> Images { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }
        [JsonProperty("release_date_precision")]
        public string ReleaseDatePrecision { get; set; }
        [JsonProperty("total_tracks")]
        public int TotalTracks { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
}
