namespace Servicio.RetazoMarket.Api.Models.Common;

public class ApiErrorResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public IReadOnlyCollection<string> Errors { get; set; } = Array.Empty<string>();
    public string? TraceId { get; set; }

    public static ApiErrorResponse Fail(
        string message,
        IReadOnlyCollection<string>? errors = null,
        string? traceId = null)
    {
        return new ApiErrorResponse
        {
            Success = false,
            Message = message,
            Errors = errors ?? Array.Empty<string>(),
            TraceId = traceId
        };
    }
}
