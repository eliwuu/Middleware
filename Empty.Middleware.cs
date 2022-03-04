internal class EmptyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly int _counter;
    private readonly ILogger _logger;

    public EmptyMiddleware(RequestDelegate next, ILogger logger, int counter)
    {
        _next = next;
        _logger = logger;
        _counter = counter;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
#if DEBUG
        _logger.Log(LogLevel.Information, _counter.ToString());
#endif
        await _next(httpContext);
    }
}