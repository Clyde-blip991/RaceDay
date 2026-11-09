using System.Net.Http.Json;

namespace RaceDay.Web.Services
{
    public class ApiClient
    {
        private readonly IHttpClientFactory _factory;
        private readonly string _baseUrl;

        public ApiClient(IHttpClientFactory factory, IConfiguration config)
        {
            _factory = factory;
            _baseUrl = config["ApiBaseUrl"] ?? "http://localhost:5075/";
        }

        private HttpClient Client
        {
            get
            {
                var client = _factory.CreateClient();
                client.BaseAddress = new Uri(_baseUrl);
                return client;
            }
        }

        public async Task<(bool ok, int status, string body)> PostAsync(string path, object payload, int? userId = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, path)
            {
                Content = JsonContent.Create(payload)
            };
            if (userId.HasValue)
                request.Headers.Add("X-User-Id", userId.Value.ToString());

            var response = await Client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, (int)response.StatusCode, body);
        }

        public async Task<(bool ok, int status, string body)> GetAsync(string path, int? userId = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, path);
            if (userId.HasValue)
                request.Headers.Add("X-User-Id", userId.Value.ToString());

            var response = await Client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, (int)response.StatusCode, body);
        }
    }
}