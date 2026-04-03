namespace Zentry.Application.Common;

public class ApiResponse<T>
{
    public bool Ok { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new();

    public static ApiResponse<T> Success(T data, string message = "Operación exitosa")
        => new() { Ok = true, Message = message, Data = data };

    public static ApiResponse<T> Fail(string message, params string[] errors)
        => new() { Ok = false, Message = message, Errors = errors.ToList() };
}
