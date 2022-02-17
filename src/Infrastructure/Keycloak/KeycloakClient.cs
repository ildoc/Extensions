using System.Net.Http.Headers;
using System.Web;
using CSharpFunctionalExtensions;
using Extensions;
using Infrastructure.Exceptions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Infrastructure.Keycloak
{
    public class KeycloakClient : IKeycloakClient
    {
        private readonly KeycloakSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public KeycloakClient(IHttpClientFactory httpClientFactory, IOptions<KeycloakSettings> settings)
        {
            _settings = settings.Value;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetTokenUsingClientCredentials()
        {
            //get token using client credentials
            var postData = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", _settings.ClientId },
                { "client_secret", _settings.ClientSecret }
            };

            var json = await HttpPostAndGetResponse<object>(postData, $"{_settings.KeycloakBaseUrl}realms/{_settings.RealmName}/protocol/openid-connect/token");

            var token = ((JObject)json.Value)["access_token"].ToString();
            if (token.IsNullOrEmpty())
            {
                throw new BadRequestException("Access token not valid");
            }
            return token;
        }

        public async Task<bool> CheckToken(string token)
        {
            var postData = new Dictionary<string, string>
            {
                { "client_id", _settings.ClientId },
                { "client_secret", _settings.ClientSecret },
                { "token", token ?? "" },
                { "token_type", "access_token" }
            };

            var json = await HttpPostAndGetResponse<object>(postData, $"{_settings.KeycloakBaseUrl}realms/{_settings.RealmName}/protocol/openid-connect/token/introspect");
            return json.OnSuccessTry(res => ((JObject)res)["active"].ToString().ToBool()).Value;
        }

        public async Task<KeycloackGetUserResponse> GetUserInfo(string mail)
        {
            var token = await GetTokenUsingClientCredentials();
            using var httpClient = _httpClientFactory.CreateClient();

            httpClient.DefaultRequestHeaders.Add("app_name", _settings.UserServiceAppName);
            httpClient.DefaultRequestHeaders.Add("app_key", _settings.UserServiceAppKey);
            //var encodedMail = HttpUtility.UrlEncode(mail);
            httpClient.DefaultRequestHeaders.Add("email", mail);

            //get userInfo
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //var response = await httpClient.GetAsync(_settings.UserByMailUrl + encodedMail);
            var response = await httpClient.GetAsync(_settings.UserServiceUrl);

            var res = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<KeycloackGetUserResponse>(res);

            return user ?? throw new EntityNotFoundException("User not found");
        }

        public async Task<KeycloackGetUserResponse> GetUserInfoByUsername(string username)
        {
            var token = await GetTokenUsingClientCredentials();
            using var httpClient = _httpClientFactory.CreateClient();

            //get userInfo
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var encodedUsername = HttpUtility.UrlEncode(username);
            var response = await httpClient.GetAsync(_settings.UserByUsernameUrl + encodedUsername);

            var res = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<IEnumerable<KeycloackGetUserResponse>>(res)?.FirstOrDefault();

            return user ?? throw new EntityNotFoundException("User not found");
        }

        public async Task<Result> ChangeUserPassword(string userId)
        {
            var token = await GetTokenUsingClientCredentials();

            var url = $"{_settings.KeycloakBaseUrl}admin/realms/{_settings.RealmName}/users/{userId}/execute-actions-email";

            var res = await HttpPutAndGetResponse<object>("[\"UPDATE_PASSWORD\"]", url, token);

            return res;
        }

        public async Task<Result> ChangeMail(string mail, string userId)
        {
            var token = await GetTokenUsingClientCredentials();
            var url = $"{_settings.KeycloakBaseUrl}admin/realms/{_settings.RealmName}/users/{userId}";

            var json = JObject.FromObject(new
            {
                email = mail,
                emailVerified = true,
            }).ToString();

            var res = await HttpPutAndGetResponse<object>(json, url, token).ConfigureAwait(false);

            return res;
        }

        public async Task<Result> CreateUser(string userName, string email, string firstName, string lastName, Dictionary<string, string> attributes)
        {
            var token = await GetTokenUsingClientCredentials();

            var client = new RestClient(_settings.KeycloakBaseUrl);
            var request = new RestRequest($"admin/realms/{_settings.RealmName}/users");
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            request.RequestFormat = DataFormat.Json;

            request.AddJsonBody(new
            {
                username = userName,
                email,
                firstName,
                lastName,
                emailVerified = true,
                enabled = true,
                attributes,
            });

            try
            {
                var res = await client.PostAsync<object>(request);

                return Result.Success(res);
            }
            catch (Exception e)
            {
                return Result.Failure(e.Message);
            }
        }

        public async Task<Result> ImpersonateUser(string userId, string token)
        {
            var ttoken = await GetTokenUsingClientCredentials();
            var client = new RestClient(_settings.KeycloakBaseUrl);

            var request = new RestRequest($"admin/realms/{_settings.RealmName}/users/{userId}/impersonation");
            request.AddParameter("Authorization", "Bearer " + ttoken, ParameterType.HttpHeader);

            try
            {
                var res = await client.PostAsync<object>(request);

                return Result.Success(res);
            }
            catch (Exception e)
            {
                return Result.Failure(e.Message);
            }
        }

        private async Task<Result<T>> HttpPostAndGetResponse<T>(
            Dictionary<string, string> postData,
            string url)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            using var content = new FormUrlEncodedContent(postData);
            content.Headers.Clear();
            content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            var response = await httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                Result.Failure(await response.Content.ReadAsStringAsync());
            }

            var res = await response.Content.ReadAsStringAsync();

            return Result.Success(JsonConvert.DeserializeObject<T>(res));
        }

        private async Task<Result> HttpPutAndGetResponse<T>(
            string data,
            string url,
            string token)
        {
            using var httpClient = _httpClientFactory.CreateClient();

            using var content = new StringContent(data);

            content.Headers.Clear();
            content.Headers.Add("Content-Type", "application/json");
            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            }

            var response = await httpClient.PutAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var r = await response.Content.ReadAsStringAsync();
                return Result.Failure(r);
            }

            var res = await response.Content.ReadAsStringAsync();

            return Result.Success(JsonConvert.DeserializeObject<T>(res));
        }
    }
}
