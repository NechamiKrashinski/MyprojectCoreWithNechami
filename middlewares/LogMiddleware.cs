using System.Diagnostics;
using System.Text;
using Serilog;

namespace project.middleware;

public static class LoggerSetup
{
    public static void ConfigureLogger()
    {
        var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");
        Console.WriteLine($"Log Directory: {logDirectory}"); // הדפסת הנתיב

        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        Log.Logger = new LoggerConfiguration()
            // .WriteTo.Console(
            //     outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}"
            // )
            .WriteTo.File(
                Path.Combine(logDirectory, @"{Date:yyyy}/{Date:MM}/{Date:dd}/log-.txt"),
                rollingInterval: RollingInterval.Day,
                fileSizeLimitBytes: 100 * 1024 * 1024,
                rollOnFileSizeLimit: true,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}"
            )
            .CreateLogger();

        Log.Information("This is a test log entry to check file writing.");
    }

}

public class LogMiddleware
{
    private readonly RequestDelegate next;
    private static int tabCount = 0;

    public LogMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        Log.Debug("LogMiddleware started processing request: {Path}", context.Request.Path);
        Log.Debug("LogMiddleware is attempting to write to the log file.");

        var logEntry = await CreateLogEntry(context, "start");
        tabCount++;

        Log.Debug("Attempting to write log entry to file.");
        Log.Information(logEntry);

        var sw = new Stopwatch();
        sw.Start();

        try
        {
            await next.Invoke(context);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while processing the request.");
            throw;
        }
        finally
        {
            sw.Stop();
            tabCount--;
            logEntry = await CreateLogEntry(context, "end", sw.ElapsedMilliseconds);
            Log.Information(logEntry);
        }
    }

    private async Task<string> CreateLogEntry(
        HttpContext context,
        string status,
        long? elapsedMilliseconds = null
    )
    {
        var logEntry = new StringBuilder();
        var tabs = new string(' ', tabCount * 2);

        if (status == "start")
        {
            logEntry.AppendLine($"{tabs}start: {context.Request.Path}");
            logEntry.AppendLine($"{tabs}  started:");
            logEntry.AppendLine($"{tabs}    Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            logEntry.AppendLine($"{tabs}    IP: {context.Connection.RemoteIpAddress}");
            logEntry.AppendLine($"{tabs}    Query: {context.Request.QueryString}");
            logEntry.AppendLine($"{tabs}    Body: {await ReadRequestBody(context)}");
        }
        else if (status == "end" && elapsedMilliseconds.HasValue)
        {
            logEntry.AppendLine(
                $"{tabs}end: {context.Request.Path} took {elapsedMilliseconds.Value} ms"
            );
            logEntry.AppendLine($"{tabs}  Completed:");
            logEntry.AppendLine($"{tabs}    Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            logEntry.AppendLine($"{tabs}    Path: {context.Request.Path}");
        }

        return logEntry.ToString();
    }

    private async Task<string> ReadRequestBody(HttpContext context)
    {
        context.Request.EnableBuffering();
        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
        {
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
            return body;
        }
    }
}

public static partial class MiddlewareExtensions
{
    public static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LogMiddleware>();
    }
}
