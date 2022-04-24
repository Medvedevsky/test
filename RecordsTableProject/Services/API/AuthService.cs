using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RecordsTable.Models;
using RecordsTable.Services.API.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RecordsTable.Services.API
{
    public class AuthService
    {
        private IWritableOptions<Settings> _options;
        public AuthService(IWritableOptions<Settings> options)
        {
            _options = options;
        }

        public HttpClient CreateHttpClient()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_options.Value.Uri);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(_options.Value.AcceptHeader));
            string authHeaders = $"Bearer {_options.Value.BearerToken}, User {_options.Value.UserToken}";
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authHeaders);

            return httpClient;
        }

        public void Auth(string url)
        {
            HttpClient httpClient = CreateHttpClient();

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            string requestData = JsonConvert.SerializeObject(new { Login = _options.Value.Login, Password = _options.Value.Password }, serializerSettings);


            HttpRequestMessage message = new(HttpMethod.Post, _options.Value.Uri + url);
            message.Headers.Add("Accept", _options.Value.AcceptHeader);
            message.Content = new StringContent(requestData);
            message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = httpClient.SendAsync(message).Result;

            string responseContent = response.Content.ReadAsStringAsync().Result;


            if (response.IsSuccessStatusCode)
            {
                Response<User> data = JsonConvert.DeserializeObject<Response<User>>(responseContent);
                User user = data.Data;
                _options.Update(opt => opt.UserToken = user.UserToken);
            }
        }
    }
}
