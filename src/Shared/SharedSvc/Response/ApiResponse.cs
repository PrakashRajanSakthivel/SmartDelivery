using System.Text.Json.Serialization;

namespace SharedSvc.Response
{
    public class ApiResponse<T>
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("data")]
        public T? Data { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("errors")]
        public List<ApiError>? Errors { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // Success response constructor
        public static ApiResponse<T> SuccessM(T data, string? message = null)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message
            };
        }

        // Error response constructor
        public static ApiResponse<T> Error(string message, List<ApiError>? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }

        // Validation error response constructor
        public static ApiResponse<T> ValidationError(List<ApiError> errors)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = "Validation failed",
                Errors = errors
            };
        }
    }

    public class ApiError
    {
        [JsonPropertyName("property")]
        public string? Property { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string? Code { get; set; }

        public ApiError(string message, string? property = null, string? code = null)
        {
            Message = message;
            Property = property;
            Code = code;
        }
    }
}
