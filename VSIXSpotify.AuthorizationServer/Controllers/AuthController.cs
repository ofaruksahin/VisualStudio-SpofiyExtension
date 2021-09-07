using DionysosFX.Module.WebApi;
using DionysosFX.Swan.Routing;
using System.Collections.Generic;

namespace VSIXSpotify.AuthorizationServer.Controllers
{
    public class AuthController : WebApiController
    {
        private string RedirectUri = "";
        private string ClientId = "";
        private string ClientSecret = "";
        private string SpotifyUrl = "";
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
            Context.Response.Redirect("http://www.google.com");
            Context.SetHandled();
        }
    }
}
