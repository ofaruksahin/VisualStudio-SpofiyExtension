using System.Threading.Tasks;

namespace VSIXSpotify.AddIn.Core.IRepository
{
    public interface IAuthService
    {
        string GetSpotifyFilePath();
        bool IsAuthenticated();
        Task<bool> GetToken(string code);
        Task<bool> RefreshToken();
        string GetToken();
        void DeleteFile();
    }
}
