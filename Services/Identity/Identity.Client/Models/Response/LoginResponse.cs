using MCF.Core.Auth.Models;
using MCF.Core.Http.Models;

namespace MCF.Identity.Client.Models.Response;

public record LoginResponse : SuccessResponse
{
    public CookieUser? User { get; set; }

    public static LoginResponse Failed(string message)
    {
        return new LoginResponse()
        {
            Success = false,
            Message = message
        };
    }
}