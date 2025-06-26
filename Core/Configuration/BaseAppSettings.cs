using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Hosting;

namespace MCF.Core.Configuration;

/// <summary>
/// Common app/env variables for all microservices.
/// </summary>
public abstract class BaseAppSettings
{
  [Required]
  public required string Environment { get; init; } = Environments.Development;

  [Required]
  public required string ServiceName { get; init; }

  public ConnectionStrings? ConnectionStrings { get; set; }

  public RetryPolicy RetryPolicy { get; set; } = new();
}

public class ConnectionStrings
{
  /// <summary>
  /// Microservice database URL.
  /// </summary>
  [Required]
  public required string DefaultConnection { get; set; }
}

/// <summary>
/// Asynchronous communication policy.
/// </summary>
public class RetryPolicy
{
  [Range(1, int.MaxValue)]
  public int MaxAttempts { get; set; } = 3;

  [Range(0, int.MaxValue)]
  public int DelayMs { get; set; } = 500;
}
