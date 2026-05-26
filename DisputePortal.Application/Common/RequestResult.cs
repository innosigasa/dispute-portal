namespace DisputePortal.Application.Common;

public record RequestResult<T> : IRequestResult<T>
{
    public bool IsSuccessful { get; init; }
    public int StatusCode { get; init; }
    public T? Data { get; init; }
    public string? Error { get; init; }
    public string? Message { get; init; }

    public static RequestResult<T> Ok(T data)
        => new() { IsSuccessful = true, StatusCode = 200, Data = data };

    public static RequestResult<T> Created(T data)
        => new() { IsSuccessful = true, StatusCode = 201, Data = data };

    public static RequestResult<T> NotFound(string error = "Resource not found.")
        => new() { IsSuccessful = false, StatusCode = 404, Error = error };

    public static RequestResult<T> BadRequest(string error)
        => new() { IsSuccessful = false, StatusCode = 400, Error = error };

    public static RequestResult<T> Conflict(string error)
        => new() { IsSuccessful = false, StatusCode = 409, Error = error };

    public static RequestResult<T> Unauthorized(string error = "Unauthorized.")
        => new() { IsSuccessful = false, StatusCode = 401, Error = error };

    public static RequestResult<T> ServerError(string error = "An unexpected error occurred.")
        => new() { IsSuccessful = false, StatusCode = 500, Error = error };

    public static RequestResult<T> NoContent()
        => new() { IsSuccessful = true, StatusCode = 204 };
}
