using System.Text;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

#if DEBUG
var logger = new LoggerConfiguration().WriteTo.File($"{(DateTime.Now).ToString().Replace(" ", "_")}-debug.log").CreateLogger();
builder.Logging.AddSerilog(logger);
#endif

var app = builder.Build();

#if DEBUG
app.Logger.Log(LogLevel.Information, "Debug mode");
#endif

app.Logger.Log(LogLevel.Information, args.Length > 0 ? args[0] + " middleware" : "no args");

int.TryParse(args[0], out int n);

app.UseHttpsRedirection();
var counter = 0;
while (n-- > 0)
{
    app.Use(async (context, next) =>
    {
        var ctx = new EmptyMiddleware(next, app.Logger, counter++);
        await ctx.InvokeAsync(context);
    });
}

app.Use(async (context, next) =>
{
    try
    {
        var ctx = new HelloMiddleware(next, app.Logger);
        await ctx.InvokeAsync(context);
    }
    catch (Exception ex)
    {
        app.Logger.Log(LogLevel.Error, ex.ToString());
    }
});

app.MapGet("/{foo=bar[baz]}",
    async (HttpContext context) =>
        await context.Response.BodyWriter.WriteAsync(Encoding.UTF8.GetBytes("\nHello from /"))
);

app.MapGet("/index",
    async (HttpContext context) =>
        await context.Response.BodyWriter.WriteAsync(Encoding.UTF8.GetBytes("\nHello from Index")
));

app.Run();
