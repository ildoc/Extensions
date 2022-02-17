using System.Net.Http.Headers;
using CSharpFunctionalExtensions;
using Extensions;
using Newtonsoft.Json;

namespace Infrastructure.Http
{
    public class HttpManager : IHttpManager
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpManager(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Result<T>> Get<T>(string url, Dictionary<string, string> headerParams)
        {
            using var httpClient = _httpClientFactory.CreateClient();

            headerParams.Each(p =>
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation(p.Key, p.Value));

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                Result.Failure(await response.Content.ReadAsStringAsync());
            }

            var res = await response.Content.ReadAsStringAsync();

            return Result.Success(JsonConvert.DeserializeObject<T>(res));
        }

        public async Task<Result<T>> Post<T>(string url, Dictionary<string, string> postData)
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

        public async Task<Result> Put<T>(string data, string url, string token)
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
