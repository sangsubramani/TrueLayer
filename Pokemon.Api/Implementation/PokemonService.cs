using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Pokemon.Api.HttpClient;
using Pokemon.Api.Interfaces;
using Pokemon.Api.Response;
using Pokemon.Persistency;
using Pokemon.Persistency.Providers;


namespace Pokemon.Api.Implementation
{
    public class PokemonService : BaseHttpClient, IPokemonService
    {
        private readonly ILogger<PokemonService> _logger;
        private readonly IDataProvider _dataProvider;
        private readonly IMapper _mapper;


        //Should be move to App settings for production
        private const string BASE_YODA_URL = "https://api.funtranslations.com/translate/yoda.json?=";
        private const string BASE_SHAKESPEAR_URL = "https://api.funtranslations.com/translate/shakespeare.json?=";

        public PokemonService
            (IDataProvider dataProvider,
            ILogger<PokemonService> logger,
            IMapper mapper) : base(logger)
        {
            _dataProvider = dataProvider;
            _mapper = mapper;
            _logger = logger;
        }
        public ApiResponse GetDetails(string name)
        {
            var hasValidData = HasValidData(name);

            //This check can be included or excluded depending on the level of log info required 
            if (hasValidData)
            {
                _logger.LogError("Invalid Url");
                return new ErrorResponse { StatusCode = "404", ErrorMsg = "Url Not found" };
            }

            var data = _dataProvider.GetData(name);

            if (data == null)
            {
                _logger.LogError("No data returned");
                return new ErrorResponse { StatusCode = "400", ErrorMsg = "No data returned for request" };
            }
            var pokemonResult = _mapper.Map<PokemonResponse>(data);

            return pokemonResult;
        }


        // There is potential for refactor 

        public async Task<ApiResponse> GetTranslatedDetails(string name)
        {
            var hasValidData = HasValidData(name);
            
            //This check can be included or excluded depending on the level of log info required 
            if (hasValidData)
            {
                _logger.LogError("Invalid Url");
                return new ErrorResponse { StatusCode = "404", ErrorMsg = "Url Not found" };
            }

            var data = _dataProvider.GetData(name);
            
            if (data == null)
            {
                _logger.LogError("No data returned for request");
                return new ErrorResponse { StatusCode = "400", ErrorMsg = "No data returned for request" };
            }

            var response = await GetResponseDTo(data);

            if (response.Contents == null)
            {
                _logger.LogError("There was an exception from the Api");
                return new ErrorResponse {StatusCode = "429", ErrorMsg = "Rate limiting exceeded"};
            }

            data.Description = response.Contents.Translated;

            var pokemonResult = _mapper.Map<PokemonResponse>(data);

            return pokemonResult;
        }

        private async Task<ResponseDTo> GetResponseDTo(Persistency.Pokemon data)
        {
            var values = new Dictionary<string, string> {{"text", data.Description}};
            var content = new FormUrlEncodedContent(values);

            var resultContent = await BuildRequest(data, content);

            var response = GetResponse<ResponseDTo>(resultContent);
            return response;
        }

        private async Task<string> BuildRequest(Persistency.Pokemon data, FormUrlEncodedContent content)
        {
            if (CheckDataNeedsYodaTranslation(data))
            {
                //yoda translation
                return await PostRequest(BASE_YODA_URL, content);
            }

            return await PostRequest(BASE_SHAKESPEAR_URL, content);
        }

        private static bool CheckDataNeedsYodaTranslation(Persistency.Pokemon data)
        {
            return data.IsLegendary || data.HabitantType == HabitantType.Cave;
        }

        private bool HasValidData(string name)
        {
            return string.IsNullOrWhiteSpace(name);
        }
    }
}
