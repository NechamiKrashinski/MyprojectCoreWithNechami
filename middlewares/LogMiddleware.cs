using System.Diagnostics;
using System.IO;
using System.Threading;

namespace project.middleware
{
    public class LogMiddleware
    {
        private readonly RequestDelegate next;
        private readonly string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "log.txt");
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public LogMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var logEntry = $"start: {DateTime.Now} {context.Request.Path}.{context.Request.Method}";
            await LogToFile(logEntry);

            var sw = new Stopwatch();
            sw.Start();
            await next.Invoke(context);
            logEntry = $"end: {DateTime.Now} {context.Request.Path}.{context.Request.Method} took {sw.ElapsedMilliseconds} ms";
            await LogToFile(logEntry);
        }

        private async Task LogToFile(string logEntry)
        {
            await semaphore.WaitAsync();
            try
            {
                using (var writer = new StreamWriter(logFilePath, true))
                {
                    await writer.WriteLineAsync(logEntry);
                }
            }
            finally
            {
                semaphore.Release();
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
