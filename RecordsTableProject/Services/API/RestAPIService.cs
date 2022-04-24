using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RecordsTable.Services.API.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RecordsTable.Services.API
{
    public class RestAPIService
    {
        private AuthService _authService;
        private IOptions<Settings> _options;
        public RestAPIService(AuthService authService, IOptions<Settings> options)
        {
            _authService = authService;
            _options = options;
        }

        public async Task<T?> Post<T>(string url, object body)
        {
            HttpClient httpClient = _authService.CreateHttpClient();

            HttpRequestMessage message = new(HttpMethod.Post, _options.Value.Uri + url);
            message.Headers.Add("Accept", _options.Value.AcceptHeader);

            if (body != null)
            {
                var serializerSettings = new JsonSerializerSettings();
                serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                string requestData = JsonConvert.SerializeObject(body, serializerSettings);

                message.Content = new StringContent(requestData);
                message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }


            HttpResponseMessage response = await httpClient.SendAsync(message);

            string responseContent = await response.Content.ReadAsStringAsync();


            if (response.IsSuccessStatusCode)
            {
                Response<T> data = JsonConvert.DeserializeObject<Response<T>>(responseContent);
                T model = data.Data;
                return model;
            }

            return default;
        }

        public async Task<T?> Get<T>(string url, object body)
        {
            HttpClient httpClient = _authService.CreateHttpClient();

            HttpRequestMessage message = new(HttpMethod.Get, _options.Value.Uri + url);
            message.Headers.Add("Accept", _options.Value.AcceptHeader);

            if (body != null)
            {
                var serializerSettings = new JsonSerializerSettings();

                serializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                };

                string requestData = JsonConvert.SerializeObject(body, serializerSettings);

                message.Content = new StringContent(requestData);
                message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            HttpResponseMessage response = await httpClient.SendAsync(message);

            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Response<T> data = JsonConvert.DeserializeObject<Response<T>>(responseContent);
                T model = data.Data;
                return model;
            }

            return default;
        }
    }
}
