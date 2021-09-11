using Newtonsoft.Json;
using System.Collections.Generic;

namespace VSIXSpotify.AddIn.Core.Spotify
{
    public class CurrentPlaybackState
    {
        [JsonProperty("device")]
        public Device Device { get; set; }
        [JsonProperty("shuffle_state")]
        public bool ShuffleState { get; set; }
        [JsonProperty("repeat_state")]
        public string RepeatState { get; set; }
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
        [JsonProperty("context")]
        public Context _Context { get; set; }
        [JsonProperty("progress_ms")]
        public int ProgressMs { get; set; }
        [JsonProperty("item")]
        public Item _Item { get; set; }
        [JsonProperty("currently_playing_type")]
        public string CurrentlyPlayingType { get; set; }
        [JsonProperty("actions")]
        public Action Action { get; set; }
        [JsonProperty("is_playing")]
        public bool IsPlaying { get; set; }

        public class Context
        {
            [JsonProperty("external_urls")]
            public ExternalUrl ExternalUrl { get; set; }
            [JsonProperty("href")]
            public string Href { get; set; }
            [JsonProperty("type")]
            public string Type { get; set; }
            [JsonProperty("uri")]
            public string Uri { get; set; }
        }

        public class Item
        {
            [JsonProperty("album")]
            public Album Album { get; set; }
            [JsonProperty("artists")]
            public List<Artist> Artists{ get; set; }
            [JsonProperty("disc_number")]
            public int DiscNumber { get; set; }
            [JsonProperty("duration_ms")]
            public int DurationMs { get; set; }
            [JsonProperty("explicit")]
            public bool Explicit { get; set; }
            [JsonProperty("external_ids")]
            public ExternalId ExternalId{ get; set; }
            [JsonProperty("external_urls")]
            public ExternalUrl ExternalUrl { get; set; }
            [JsonProperty("href")]
            public string Href { get; set; }
            [JsonProperty("id")]
            public string Id { get; set; }
            [JsonProperty("is_local")]
            public bool IsLocal { get; set; }
            [JsonProperty("is_playable")]
            public bool IsPlayable { get; set; }
            [JsonProperty("linked_from")]
            public LinkedFrom LinkedFrom { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("popularity")]
            public int Popularity { get; set; }
            [JsonProperty("preview_url")]
            public string PreviewUrl { get; set; }
            [JsonProperty("track_number")]
            public int TrackNumber { get; set; }
            [JsonProperty("type")]
            public string Type { get; set; }
            [JsonProperty("uri")]
            public string Uri { get; set; }
        }
    }
}
