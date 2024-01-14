using System.Net;
using System.Text.Json;
using MySqlConnector;
using Serilog;
using Shared;
using Shared.Modals;

namespace Api;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        this._next = next;
    }
    
    private readonly Serilog.ILogger _logger = new LoggerConfiguration()
        .WriteTo.File("Logs/Logs.txt", rollingInterval: RollingInterval.Day)
        .WriteTo.Console()
        .CreateLogger();

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var responseModel = await ApiResponseModal<object>.FatalAsync(error, BaseErrorCodes.UnknownSystemException, _logger);

            switch (error)
            {
                case ApplicationException:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case MySqlException e:
                    responseModel =
                        await ApiResponseModal<object>.FatalAsync(e, BaseErrorCodes.DatabaseUnknownError, _logger);
                    break;

                case KeyNotFoundException:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                default:
                    // unhandled error
                    if (error.HResult == 401)
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    else
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            var result = JsonSerializer.Serialize(responseModel);
            await response.WriteAsync(result);
        }
    }
}