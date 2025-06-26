using System.ComponentModel.DataAnnotations;
using MCF.Core.Configuration;

namespace MCF.CurrencyRate.API;

public class AppSettings : BaseAppSettings
{
    [Required]
    public required string CurrencyAPIKey { get; set; }
}
