namespace DisputePortal.Application.Common;

public interface IRequestResult
{
    bool IsSuccessful { get; }
    int StatusCode { get; }
    string? Error { get; }
    string? Message { get; }
}

public interface IRequestResult<T> : IRequestResult
{
    T? Data { get; }
}
