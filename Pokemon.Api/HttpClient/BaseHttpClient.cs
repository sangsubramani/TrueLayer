using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Pokemon.Api.HttpClient
{
    public abstract class BaseHttpClient
    {
        private readonly ILogger<BaseHttpClient> _logger;

        protected BaseHttpClient(ILogger<BaseHttpClient> logger)
        {
            _logger = logger;
        }

        protected async Task<string> PostRequest(string url, FormUrlEncodedContent content)
        {
            using var client = new System.Net.Http.HttpClient();
            try
            {
                var response = await client.PostAsync(url, content);

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("There was an exception from the external Api", ex);
                return HttpStatusCode.BadRequest.ToString();
            }
        }

        protected T GetResponse<T>(string resultContent)
        {
            var response = JsonConvert.DeserializeObject<T>(resultContent);

            return response;
        }
    }
}
