using System.ComponentModel.DataAnnotations;
using MCF.Core.Configuration;

namespace MCF.Identity.API;

public class AppSettings : BaseAppSettings
{
    [Required]
    public required FirebaseSettings Firebase { get; set; }

    [Required]
    public required SessionJwtSettings SessionJwt { get; set; }
}

public class FirebaseSettings
{
    [Required]
    public required string ProjectId { get; set; }
}

public class SessionJwtSettings
{
    [Required]
    public required string Secret { get; set; }

    [Required]
    public required string Issuer { get; set; }

    [Required]
    public required string Audience { get; set; }
}