using BusinessMeet.Api.Moddels;
using BusinessMeet.Service.Exceptions;

namespace BusinessMeet.Api.Middlewares;

public class ExceptionHandlerMiddleWare
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionHandlerMiddleWare(RequestDelegate next, ILogger<ExceptionHandlerMiddleWare> logger)
    {
        this._next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BusinessMeetException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            await context.Response.WriteAsJsonAsync(new Response
            {
                StatusCode = ex.StatusCode,
                Message = ex.Message,
            });
        }
        catch (Exception ex)
        {
            this._logger.LogError($"{ex}\n\n");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new Response
            {
                StatusCode = 500,
                Message = ex.Message,
            });
        }
    }
}

