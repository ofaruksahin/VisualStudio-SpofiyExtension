namespace VSIXSpotify.AddIn.Core.IRepository
{
    public interface IAuthService
    {
        bool IsAuthenticated();
        string GetToken();
    }
}
