using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Common.Api.Dtos;
using Newtonsoft.Json;
using Plugin.Settings;
using TD.Api.Dtos;

namespace Fourplaces.Models
{

    public class ApiClient
    {

        public const string TOKEN = nameof(TOKEN);
        public const string REFRESH_TOKEN = nameof(REFRESH_TOKEN);

        public static string Refresh_Token { get; private set; }
        public static string Token { get; private set; }


        private HttpClient _client;

        public ApiClient() { _client = new HttpClient(); }

        public async Task<HttpResponseMessage> Execute(HttpMethod method, string url, object data = null, string token = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(method, url);

            if (data != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            }

            if (token != null)
            {
                request.Headers.Add("Authorization", $"Bearer {token}");
            }
            return await _client.SendAsync(request);
        }

        public async Task<T> ReadFromResponse<T>(HttpResponseMessage response)
        {
            string result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }

        private static void saveRefreshToken()
        {
        }

        public static void setRefreshToken(string refresh_token, string token)
        {
            Token = token;
            Refresh_Token = refresh_token;

            saveRefreshToken();
        }



        public static async Task Refresh()
        {

            System.Diagnostics.Debug.WriteLine(ApiClient.Token);

            ApiClient api = new ApiClient();

            HttpResponseMessage httpResponse = await api.Execute(HttpMethod.Post,
               "https://td-api.julienmialon.com/auth/refresh",
               new RefreshRequest()
               {
                   RefreshToken = Refresh_Token
               });

            Response<LoginResult> response = await api.ReadFromResponse<Response<LoginResult>>(httpResponse);

            if (response.IsSuccess)
            {
                Token = response.Data.AccessToken;
                Refresh_Token = response.Data.RefreshToken;
                //Barrel.Current.Add("Token", _token, TimeSpan.FromSeconds(response.Data.ExpiresIn));
                System.Diagnostics.Debug.WriteLine(ApiClient.Token);

                saveRefreshToken();
                return;

            }
            else
            {
                System.Diagnostics.Debug.WriteLine(response.ErrorMessage);
            }





        }
    }
}