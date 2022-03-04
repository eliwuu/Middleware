using System.Text;

internal class HelloMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public HelloMiddleware(RequestDelegate next, ILogger logger)
    { _next = next; _logger = logger; }
    public async Task InvokeAsync(HttpContext httpContext)
    {
        var request = httpContext.Request.Query;
        var requestContent = request != null ? (request.Select(x => x.ToString()).ToList()) : null;
        var requestData = requestContent != null ? string.Join(", ", requestContent) ?? "no data" : "no data";


        var responseContent = $"Hello World from HelloMiddleware!\nRequest query: {string.Join("", requestData)}";
        var responseData = Encoding.UTF8.GetBytes(responseContent);

        try
        {
            await httpContext.Response.BodyWriter.WriteAsync(responseData);
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, ex.ToString());
        }

        await _next(httpContext);
    }
}