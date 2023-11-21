namespace Hyperspan.Shared.Modals
{
    public class ApiResponseModal
    {
        public string? ErrorCode { get; internal set; }
        public string? Message { get; internal set; }
        public bool Succeeded { get; internal set; }

        public ApiResponseModal(bool succeeded, string? errorCode = null)
        {
            Succeeded = succeeded;
            ErrorCode = errorCode;

            // TODO: Get Message for the error code provided.
        }

        public static async Task<ApiResponseModal> SuccessAsync()
            => await Task.FromResult(new ApiResponseModal(true));

        public static async Task<ApiResponseModal> FailedAsync(string errorCode)
            => await Task.FromResult(new ApiResponseModal(false, errorCode));

        public static async Task<ApiResponseModal> FatalAsync(Exception exception)
            // TODO: Log and do something with the exception
            => await Task.FromResult(new ApiResponseModal(false, BaseErrorCodes.UnknownSystemException));

        public static async Task<ApiResponseModal> FatalAsync(Exception exception, string errorCode)
            // TODO: Log and do something with the exception
            => await Task.FromResult(new ApiResponseModal(false, errorCode));

        public static async Task<ApiResponseModal> FatalAsync(string exception)
            // TODO: Log and do something with the exception
            => await Task.FromResult(new ApiResponseModal(false, BaseErrorCodes.UnknownSystemException));

    }

    public sealed class ApiResponseModal<T> : ApiResponseModal
    {
        public T? Data { get; internal set; }

        public ApiResponseModal(bool succeeded, string? errorCode = null)
            : base(succeeded, errorCode)
        {
            Data = default;
        }

        public ApiResponseModal(T data, bool succeeded, string? errorCode = null)
            : base(succeeded, errorCode)
        {
            Data = data;
        }

        public static async Task<ApiResponseModal<T>> SuccessAsync(T data)
            => await Task.FromResult(new ApiResponseModal<T>(data, true));

        public static async Task<ApiResponseModal<T>> FailedAsync(string errorCode)
            => await Task.FromResult(new ApiResponseModal<T>(false, errorCode));

        public static async Task<ApiResponseModal<T>> FatalAsync(Exception exception)
            // TODO: Log and do something with the exception
            => await Task.FromResult(new ApiResponseModal<T>(false, BaseErrorCodes.UnknownSystemException));

        public static async Task<ApiResponseModal<T>> FatalAsync(Exception exception, string errorCode)
            // TODO: Log and do something with the exception
            => await Task.FromResult(new ApiResponseModal<T>(false, errorCode));

        public static async Task<ApiResponseModal<T>> FatalAsync(string exception)
            // TODO: Log and do something with the exception
            => await Task.FromResult(new ApiResponseModal<T>(false, BaseErrorCodes.UnknownSystemException));

    }

}
