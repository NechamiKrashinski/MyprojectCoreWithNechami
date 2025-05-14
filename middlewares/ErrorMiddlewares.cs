using System.Net;
using System.Net.Mail;

namespace project.middleware;

public class ErrorMiddlware
{
    private readonly RequestDelegate next;

    public ErrorMiddlware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext c)
    {
        try
        {
            await next(c);
        }
        catch (ApplicationException ex)
        {
            c.Response.StatusCode = 400;
            // Email(ex.Message);
            await c.Response.WriteAsync(ex.Message);
        }
        catch (Exception ex)
        {
            //  Email(ex.Message);
            c.Response.StatusCode = 500;
            await c.Response.WriteAsync("פנה לתמיכה הטכנית" + ex.Message);
        }
    }

    // public void Email(string error)
    // {
    //     var fromAddress = new MailAddress("n0556734722@gmail.com", "נחמי קרשינסקי");
    //     var toAddress = new MailAddress("n0556734722@gmail.com", "נחמי");
    //     const string fromPassword = "N6734722!";
    //     const string subject = "ERROR C#";
    //     var body = error;
    //     body += "\n\nThis is an automated message, please do not reply.";
    //     var smtp = new SmtpClient
    //     {
    //         Host = "smtp.gmail.com", // כתובת ה-SMTP של הספק שלך
    //         Port = 587, // או 465 אם אתה משתמש ב-SSL
    //         EnableSsl = true,
    //         DeliveryMethod = SmtpDeliveryMethod.Network,
    //         UseDefaultCredentials = false,
    //         Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
    //     };

    //     using (
    //         var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body }
    //     )
    //     {
    //         smtp.Send(message);
    //     }
    // }
}

public static partial class MiddlewareExtensions
{
    public static IApplicationBuilder UseErrorMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorMiddlware>();
    }
}
