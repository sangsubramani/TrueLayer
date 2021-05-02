using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pokemon.Api.Interfaces;
using Pokemon.Api.Response;

namespace Pokemon.Api.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    public class PokemonApi : ControllerBase
    {
        private readonly ILogger<PokemonApi> _logger;
        private readonly IPokemonService _pokemonService;
       
        public PokemonApi
            (IPokemonService pokemonService,
            ILogger<PokemonApi> logger)
        {
            _pokemonService = pokemonService;
            _logger = logger;
        }

        [HttpGet("pokemon/{name}")]
        public ActionResult<ApiResponse> Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogError("End point is incorrect,include the name at the end of the URL ");
                return BadRequest(string.Empty);
            }

            var pokemonResult = _pokemonService.GetDetails(name);

            if (pokemonResult != null) return pokemonResult;
            _logger.LogError("No data returned");
            return NotFound(string.Empty);
        }

        [HttpGet("pokemon/translated/{name}")]
        public async Task<ApiResponse> GetTranslatedDetails(string name)
        {
            var result = await _pokemonService.GetTranslatedDetails(name);
            return result;
        }
    }
}
