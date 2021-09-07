using Autofac;
using System;
using System.IO;
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

        private string GetSpotifyFilePath()
        {
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return Path.Combine(userPath, ".vsixspotify");
        }

        public bool IsAuthenticated()
        {
            var filePath = GetSpotifyFilePath();
            if (!File.Exists(filePath))
                return false;
            try
            {
                var fileContent = File.ReadAllText(filePath);
            }
            catch (Exception)
            {

            }
            return false;
        }

        public string GetToken()
        {
            return string.Empty;
        }
    }
}
