using System.Threading.Tasks;
using Pokemon.Api.Response;

namespace Pokemon.Api.Interfaces
{
    public interface IPokemonService
    {
        ApiResponse GetDetails(string name);

        Task<ApiResponse> GetTranslatedDetails(string name);
    }
}
