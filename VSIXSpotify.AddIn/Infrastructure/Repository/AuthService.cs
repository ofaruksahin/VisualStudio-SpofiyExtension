using Autofac;
using VSIXSpotify.AddIn.Core.IRepository;

namespace VSIXSpotify.AddIn.Infrastructure.Repository
{
    public class AuthService : IAuthService
    {
        ISpotifyService spotifyService = null;

        public AuthService()
        {
            ContainerHelper
                .Build()
                .TryResolve<ISpotifyService>(out spotifyService);
        }

        public bool IsAuthenticated()
        {
            return false;
        }

        public string GetToken()
        {
            return string.Empty;
        }
    }
}
