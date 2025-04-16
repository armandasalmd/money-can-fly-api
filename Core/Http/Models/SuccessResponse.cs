namespace MCF.Core.Http.Models;

public record SuccessResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
}