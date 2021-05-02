using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Pokemon.Api.Response
{
    [JsonObject]
    public class PokemonResponse : ApiResponse
    {
       [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("habitant")]
        public string HabitantType { get; set; }

        [JsonPropertyName("islegendary")]
        public bool IsLegendary { get; set; }
    }
}
