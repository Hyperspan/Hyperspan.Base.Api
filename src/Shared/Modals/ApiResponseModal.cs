using Serilog;

namespace Shared.Modals
{
    public class ApiResponseModal
    {
        public string? ErrorCode { get; internal set; }
        public string? Message { get; internal set; }
        public bool Succeeded { get; internal set; }

        protected ApiResponseModal(bool succeeded, string? errorCode = null)
        {
            Succeeded = succeeded;
            ErrorCode = errorCode;
            if (errorCode == null) return;
            new BaseErrorCodes().ErrorMessages.TryGetValue(errorCode, out var message);
            Message = message;
        }

        public static async Task<ApiResponseModal> SuccessAsync()
            => await Task.FromResult(new ApiResponseModal(true));

        public static async Task<ApiResponseModal> FailedAsync(string errorCode)
        {
            new BaseErrorCodes().ErrorMessages.TryGetValue(errorCode, out var message);
            Log.Fatal(message ?? "Some error occurred.", BaseErrorCodes.UnknownSystemException);
            return await Task.FromResult(new ApiResponseModal(false, errorCode));
        }

        public static async Task<ApiResponseModal> FatalAsync(Exception exception)
        {
            Log.Fatal(exception, BaseErrorCodes.UnknownSystemException);
            return await Task.FromResult(new ApiResponseModal(false, BaseErrorCodes.UnknownSystemException));
        }
        public static async Task<ApiResponseModal> FatalAsync(Exception exception, string errorCode)
        {
            Log.Fatal(exception, errorCode);
            return await Task.FromResult(new ApiResponseModal(false, errorCode));
        }
        public static async Task<ApiResponseModal> FatalAsync(string exception)
        {
            Log.Fatal(exception, BaseErrorCodes.UnknownSystemException);
            return await Task.FromResult(new ApiResponseModal(false, BaseErrorCodes.UnknownSystemException));
        }
    }

    public sealed class ApiResponseModal<T> : ApiResponseModal
    {
        public T? Data { get; private set; }

        private ApiResponseModal(bool succeeded, string? errorCode = null)
            : base(succeeded, errorCode)
        {
            Data = default;
        }

        private ApiResponseModal(T data, bool succeeded, string? errorCode = null)
            : base(succeeded, errorCode)
        {
            Data = data;
        }

        public static async Task<ApiResponseModal<T>> SuccessAsync(T data)
            => await Task.FromResult(new ApiResponseModal<T>(data, true));

        public new static async Task<ApiResponseModal<T>> FailedAsync(string errorCode)
        {
            new BaseErrorCodes().ErrorMessages.TryGetValue(errorCode, out var message);
            Log.Fatal(message ?? "Some Error Occurred.", errorCode);
            return await Task.FromResult(new ApiResponseModal<T>(false, errorCode));
        }
        public new static async Task<ApiResponseModal<T>> FatalAsync(Exception exception)
        {
            Log.Fatal(exception, BaseErrorCodes.UnknownSystemException);
            return await Task.FromResult(new ApiResponseModal<T>(false, BaseErrorCodes.UnknownSystemException));
        }
        public new static async Task<ApiResponseModal<T>> FatalAsync(Exception exception, string errorCode)
        {
            Log.Fatal(exception, errorCode);
            return await Task.FromResult(new ApiResponseModal<T>(false, errorCode));
        }
        public new static async Task<ApiResponseModal<T>> FatalAsync(string exception)
        {
            Log.Fatal(exception, BaseErrorCodes.UnknownSystemException);
            return await Task.FromResult(new ApiResponseModal<T>(false, BaseErrorCodes.UnknownSystemException));
        }

    }

}
