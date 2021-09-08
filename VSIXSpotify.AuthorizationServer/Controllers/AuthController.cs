using DionysosFX.Module.IWebApi;
using DionysosFX.Module.WebApi;
using DionysosFX.Module.WebApi.EnpointResults;
using DionysosFX.Swan.Routing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace VSIXSpotify.AuthorizationServer.Controllers
{
    public class AuthController : WebApiController
    {
        private string RedirectUri = ConfigurationManager.AppSettings["redirectUrl"];
        private string ClientId = ConfigurationManager.AppSettings["clientId"];
        private string ClientSecret = ConfigurationManager.AppSettings["clientSecret"];
        private string SpotifyUrl = ConfigurationManager.AppSettings["spotifyUrl"];

        private List<string> Scopes = new List<string>()
        {
            "ugc-image-upload",
            "playlist-modify-private",
             "playlist-read-private",
             "playlist-modify-public",
             "playlist-read-collaborative",
             "user-read-private",
             "user-read-email",
             "user-read-playback-state",
             "user-modify-playback-state",
             "user-read-currently-playing",
             "user-library-modify",
             "user-library-read",
             "user-read-playback-position",
             "user-read-recently-played",
             "user-top-read",
             "app-remote-control",
             "streaming",
             "user-follow-modify",
             "user-follow-read"
        };


        [Route(HttpVerb.GET, "/redirect")]
        public void Redirect()
        {
            var scopes = string.Join(' ', Scopes);
            string url = string.Format("{0}/authorize?response_type=code&client_id={1}&scope={2}&redirect_uri={3}", SpotifyUrl, ClientId, scopes, RedirectUri);
            Context.Response.Redirect(url);
            Context.SetHandled();
        }

        [Route(HttpVerb.GET, "/callback/{error}/{state}/{code}")]
        public void Callback([QueryData] string error, [QueryData] string state, [QueryData] string code)
        {
            
        }

        [Route(HttpVerb.GET,"/token/{code}")]
        public async Task<IEndpointResult> Token([QueryData] string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                try
                {
                    string url = string.Format("{0}/api/token", SpotifyUrl);
                    using (HttpClient client = new HttpClient())
                    {
                        var dict = new Dictionary<string, string>();
                        dict.Add("grant_type", "authorization_code");
                        dict.Add("code", code);
                        dict.Add("redirect_uri", RedirectUri);
                        dict.Add("client_id", ClientId);
                        dict.Add("client_secret", ClientSecret);
                        var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(dict) };
                        var res = await client.SendAsync(req);
                        res.EnsureSuccessStatusCode();
                        var content = await res.Content.ReadAsStringAsync();
                        var token = JsonConvert.DeserializeObject<TokenItem>(content);
                        return new Ok(token);
                    }
                }
                catch (System.Exception e)
                {

                }

            }
            return new NotFound(new { });
        }

        [Route(HttpVerb.GET, "/refresh/{refresh_token}")]
        public async Task<IEndpointResult> Refresh([QueryData] string refresh_token)
        {
            if (!string.IsNullOrEmpty(refresh_token))
            {
                string url = string.Format("{0}/api/token", SpotifyUrl);
                using(HttpClient client = new HttpClient())
                {
                    try
                    {
                        var dict = new Dictionary<string, string>();
                        dict.Add("grant_type", "refresh_token");
                        dict.Add("refresh_token", refresh_token);
                        dict.Add("client_id", ClientId);
                        dict.Add("client_secret", ClientSecret);
                        var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(dict) };
                        var res = await client.SendAsync(req);
                        res.EnsureSuccessStatusCode();
                        var content = await res.Content.ReadAsStringAsync();
                        var token = JsonConvert.DeserializeObject<TokenItem>(content);
                        token.refresh_token = refresh_token;
                        return new Ok(token);
                    }
                    catch (System.Exception)
                    {
                        
                    }                
                }
            }
            return new NotFound(new { });
        }
    }

    internal class TokenItem
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }
    }
}
