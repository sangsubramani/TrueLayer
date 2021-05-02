
namespace Pokemon.Api.Response
{
    public class ErrorResponse : ApiResponse
    {
        public string ErrorMsg { get; set; }

        public string StatusCode { get; set; }
    }
}
