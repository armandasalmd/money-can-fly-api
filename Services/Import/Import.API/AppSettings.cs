using System.ComponentModel.DataAnnotations;
using MCF.Core.Configuration;

namespace MCF.Import.API;

public class AppSettings : BaseAppSettings
{
    public GoCardlessSettings? GoCardless { get; set; }
}

public class GoCardlessSettings
{
    [Required]
    public required string SecretKey { get; set; }

    [Required]
    public required string SecretId { get; set; }
}
