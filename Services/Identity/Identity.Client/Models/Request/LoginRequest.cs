using System.ComponentModel.DataAnnotations;

namespace MCF.Identity.Client.Models.Request;

public class LoginRequest
{
    [Required]
    public required string UserIdToken { get; set; }
}