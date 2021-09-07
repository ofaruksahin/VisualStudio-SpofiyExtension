using DionysosFX.Module.IWebApi;
using DionysosFX.Module.WebApi;
using DionysosFX.Module.WebApi.EnpointResults;
using DionysosFX.Swan.Routing;
using System.Collections.Generic;
using System.Configuration;

namespace VSIXSpotify.AuthorizationServer.Controllers
{
    public class AuthController : WebApiController
    {
        private string RedirectUri = ConfigurationManager.AppSettings["redirectUrl"];
        private string ClientId = ConfigurationManager.AppSettings["clientId"];
        private string ClientSecret = ConfigurationManager.AppSettings["cleintSecret"];
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

        [Route(HttpVerb.GET,"/callback/{error}/{state}/{code}")]
        public IEndpointResult Callback([QueryData]string error,[QueryData]string state,[QueryData]string code)
        {
            return new Ok(new { });
        }
    }
}
