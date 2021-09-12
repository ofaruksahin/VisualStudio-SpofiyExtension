using Autofac;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using VSIXSpotify.AddIn.Core;
using VSIXSpotify.AddIn.Core.IRepository;

namespace VSIXSpotify.AddIn.Infrastructure.Repository
{
    public static class SpotifyClient
    {
        private static string spotifyUrl = Options.SpotifyUrl;
        private static IAuthService authService = null;

        private static string GetToken()
        {
            if (authService == null)
                ContainerHelper
                    .Build()
                    .TryResolve<IAuthService>(out authService);
            var isAuthenticated = authService.IsAuthenticated();
            if (isAuthenticated)
                return authService.GetToken();
            return "";
        }

        public static async Task<HttpResponseMessage> Get(string endpoint)
        {
            bool isFirstRequest = true;
        repeat:
            var token = GetToken();
            if (string.IsNullOrEmpty(token))
                return null;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var res = await client.GetAsync(string.Format("{0}{1}", spotifyUrl, endpoint));
                    if (res.StatusCode == HttpStatusCode.Unauthorized && isFirstRequest)
                    {
                        isFirstRequest = false;
                        if (await authService.RefreshToken())
                            goto repeat;
                    }
                    return res;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public static async Task<HttpResponseMessage> Post(string endpoint,object obj = null)
        {
            bool isFirstRequest = true;
        repeat:
            var token = GetToken();
            if (string.IsNullOrEmpty(token))
                return null;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var res = await client.PostAsync(string.Format("{0}{1}", spotifyUrl, endpoint),new StringContent(JsonConvert.SerializeObject(obj)));
                    if (res.StatusCode == HttpStatusCode.Unauthorized && isFirstRequest)
                    {
                        isFirstRequest = false;
                        if (await authService.RefreshToken())
                            goto repeat;
                    }
                    return res;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public static async Task<HttpResponseMessage> Put(string endpoint, object obj = null)
        {
            bool isFirstRequest = true;
        repeat:
            var token = GetToken();
            if (string.IsNullOrEmpty(token))
                return null;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var res = await client.PutAsync(string.Format("{0}{1}", spotifyUrl, endpoint), new StringContent(JsonConvert.SerializeObject(obj)));
                    if (res.StatusCode == HttpStatusCode.Unauthorized && isFirstRequest)
                    {
                        isFirstRequest = false;
                        if (await authService.RefreshToken())
                            goto repeat;
                    }
                    return res;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }
}
