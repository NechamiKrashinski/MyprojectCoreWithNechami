using System.Diagnostics;

namespace project.middleware;

public class LogMiddleware
{
    private readonly RequestDelegate next;

    //private readonly ILogger logger;

    public LogMiddleware(RequestDelegate next,ILogger<LogMiddleware> logger)
    {
        this.next=next;
       // this.logger=logger;
    }
    


    public async Task Invoke(HttpContext c)
    {
        Console.WriteLine($"start: {DateTime.Now} {c.Request.Path}.{c.Request.Method}");
        var sw = new Stopwatch();
        sw.Start();
        await next.Invoke(c);
        Console.WriteLine($"end: {DateTime.Now} {c.Request.Path}.{c.Request.Method} took{sw.ElapsedMilliseconds}");
    }
}

public static partial class MiddlewareExtensions{

    public static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LogMiddleware>();
    }
}