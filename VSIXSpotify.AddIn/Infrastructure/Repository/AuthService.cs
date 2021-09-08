using Autofac;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using VSIXSpotify.AddIn.Core;
using VSIXSpotify.AddIn.Core.IRepository;

namespace VSIXSpotify.AddIn.Infrastructure.Repository
{
    public class AuthService : IAuthService
    {
        public string GetSpotifyFilePath()
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
                var tokenItem = JsonConvert.DeserializeObject<TokenItem>(fileContent);
                if (tokenItem != null)
                    return !string.IsNullOrEmpty(tokenItem.access_token);
            }
            catch (Exception)
            {

            }
            return false;
        }

        public async Task<bool> GetToken(string code)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = string.Format("{0}/token?code={1}", Options.AuthorizationUrl, code);
                    var res = await client.GetAsync(url);
                    res.EnsureSuccessStatusCode();
                    var content = await res.Content.ReadAsStringAsync();
                    var tokenItem = JsonConvert.DeserializeObject<TokenItem>(content);
                    if (tokenItem != null)
                        if (!string.IsNullOrEmpty(tokenItem.access_token))
                        {
                            var filePath = GetSpotifyFilePath();
                            if (!File.Exists(filePath))
                                File.Create(filePath).Close();
                            File.WriteAllText(filePath, content);
                            return true;
                        }
                }
            }
            catch (Exception e)
            {

            }
            return false;
        }

        public async Task<bool> RefreshToken()
        {
            var path = GetSpotifyFilePath();
            if (!File.Exists(path))
                return false;
            var content = File.ReadAllText(path);
            var tokenItem = JsonConvert.DeserializeObject<TokenItem>(content);
            if (!string.IsNullOrEmpty(tokenItem.refresh_token))
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string url = string.Format("{0}/refresh?refresh_token={1}", Options.AuthorizationUrl, tokenItem.refresh_token);
                        var res = await client.GetAsync(url);
                        res.EnsureSuccessStatusCode();
                        var resContent = await res.Content.ReadAsStringAsync();
                        tokenItem = JsonConvert.DeserializeObject<TokenItem>(resContent);
                        if (tokenItem != null)
                            if (!string.IsNullOrEmpty(tokenItem.access_token))
                            {
                                var filePath = GetSpotifyFilePath();
                                if (!File.Exists(filePath))
                                    File.Create(filePath);
                                File.WriteAllText(filePath, content);
                                return true;
                            }
                    }
                }
                catch (Exception)
                {
                }
            }
            return false;
        }

        public string GetToken()
        {
            var path = GetSpotifyFilePath();
            try
            {
                if(File.Exists(path))
                {
                    var content = File.ReadAllText(path);
                    var tokenItem = JsonConvert.DeserializeObject<TokenItem>(content);
                    return tokenItem.access_token;
                }
            }
            catch (Exception)
            {
            }
            return string.Empty;
        }

        public void DeleteFile()
        {
            try
            {

                var path = GetSpotifyFilePath();
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch (Exception)
            {
            }
        }
    }
}
