using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Integration.API
{
    public class ReuqestBaseService<TRequest, TReponse, TFilter> : IReuqestBaseService<TRequest, TReponse, TFilter>
    {
        private readonly HttpClient _httpClient;
        private const string MEDIA_TYPE = "application/json";
        public string BaseUrl = "https://biotime.tela.com.kh/";
        public string Token = "";
        public string username = "admin";
        public string password = "biotime@tela23";
        public ReuqestBaseService()
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = new CookieContainer()
            });
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MEDIA_TYPE));
        }
        public async Task<LoginReponse> Login(LoginRequest request)
        {
            var data = await PostLogin("jwt-api-token-auth/", request);
            return data.Result;
        }
        private async Task<ObjectResult<LoginReponse>> PostLogin(string endpount, LoginRequest request)
        {
            var url = BaseUrl + endpount;
            var content = GetStringContent(request);
            var response = await _httpClient.PostAsync(url, content).ConfigureAwait(false);

            try
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<LoginReponse>(responseBody);

                if (data != null)
                    Token = data.token;
                //if (response.IsSuccessStatusCode)
                    //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", Token);

                //var stream = "[encoded jwt]";
                //var handler = new JwtSecurityTokenHandler();
                //var jsonToken = handler.ReadToken(Token);
                //var tokenS = jsonToken as JwtSecurityToken;

                //CurrentCompany = jti;
                return new ObjectResult<LoginReponse>
                {
                    Result = data,
                    success = false,
                    Status = response.StatusCode.ToString(),
                    ErrorMessage = response.ReasonPhrase
                };
            }
            catch (Exception ex)
            {
                return new ObjectResult<LoginReponse>
                {
                    success = false,
                    Status = "501",
                    ErrorMessage = ex.Message
                };
            }

        }
        public async Task<ObjectResult<TReponse>> Get(string endPoint)
        {
            try
            {
                //Token = "JWT eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoxLCJ1c2VybmFtZSI6ImFkbWluIiwiZXhwIjoxNzE3NzM0MTUzLCJlbWFpbCI6Im5hbXB1dGhlYUBnbWFpbC5jb20iLCJvcmlnX2lhdCI6MTcxNzEyOTM1M30.Y9PZa-rXrEjI4VnDoNN6P88hUOXP5McGhnz04tohUZg";
                string url = BaseUrl + endPoint;

                if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _httpClient.DefaultRequestHeaders.Add("Authorization", $"JWT {Token}");
                }

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    throw new Exception(errorContent);
                }

                string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                JObject o = JObject.Parse(responseBody);

                var data = o.ToObject<TReponse>();

                return new ObjectResult<TReponse>
                {
                    Result = data,
                    success = true,
                    Status = response.StatusCode.ToString()
                };
            }
            catch (HttpRequestException httpEx)
            {
                // Handle HTTP request specific errors
                throw new Exception("HTTP Request Error: " + httpEx.Message, httpEx);
            }
            catch (Exception ex)
            {
                // General exception handling
                throw;
            }
        }

        //public async Task<ObjectResult<TReponse>> Get(string endPoint)
        //{
        //    try
        //    {
        //        string url = BaseUrl + endPoint;
        //        _httpClient.DefaultRequestHeaders.Add("Authorization", $"JWT {Token}");
        //        var response = await _httpClient.GetAsync(url);
        //        if (!response.IsSuccessStatusCode)
        //            throw new Exception(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        //        else
        //        {
        //            string responseBody = await response.Content.ReadAsStringAsync();
        //            JObject o = JObject.Parse(responseBody);

        //            var data = o.ToObject<TReponse>();

        //            return new ObjectResult<TReponse>
        //            {
        //                Result = data,
        //                success = response.IsSuccessStatusCode,
        //                Status = response.StatusCode.ToString()
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        private StringContent GetStringContent(object obj)
        {
            var result = System.Text.Json.JsonSerializer.Serialize(obj, JsonSettings);
            var content = new StringContent(result, Encoding.UTF8, MEDIA_TYPE);
            return content;
        }

        

        public Task<ObjectResult<TReponse>> Post(string endpount, TRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ObjectResult<TReponse>> Put(string endpount, TRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ObjectResults<TReponse>> GetAll(string endpount, TFilter filter)
        {
            throw new NotImplementedException();
        }

        private JsonSerializerOptions JsonSettings = new JsonSerializerOptions
        {
            IgnoreNullValues = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
    public interface IReuqestBaseService<TRequest, TReponse, TFilter>
    {
        Task<ObjectResult<TReponse>> Get(string endpount);
        Task<ObjectResult<TReponse>> Post(string endpount, TRequest request);
        Task<ObjectResult<TReponse>> Put(string endpount, TRequest request);
        Task<ObjectResults<TReponse>> GetAll(string endpount, TFilter filter);
    }
}
