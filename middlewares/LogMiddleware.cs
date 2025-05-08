using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace project.middleware
{
    public class LogMiddleware
    {
        private readonly RequestDelegate next;
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private static int tabCount = 0; // מונה טאבים

        public LogMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // הגדל את מונה הטאבים
            var logEntry = await CreateLogEntry(context, "start");
            tabCount++;

            Log.Information(logEntry); // השתמש ב-Serilog לרישום

            var sw = new Stopwatch();
            sw.Start();
            await next.Invoke(context);
            sw.Stop();

            tabCount--;
            logEntry = await CreateLogEntry(context, "end", sw.ElapsedMilliseconds);

            Log.Information(logEntry); // השתמש ב-Serilog לרישום
        }

        private async Task<string> CreateLogEntry(
            HttpContext context,
            string status,
            long? elapsedMilliseconds = null
        )
        {
            var logEntry = new StringBuilder();
            var tabs = new string(' ', tabCount * 2); // צור את הטאבים לפי המונה

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
            using (
                var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true)
            )
            {
                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0; // Reset the stream position for the next middleware
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
}
