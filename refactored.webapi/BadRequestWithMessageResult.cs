using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

public class BadRequestWithMessageResult : IActionResult
{
    private readonly string message;

    public BadRequestWithMessageResult(string message)
    {
        this.message = message;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;
        response.StatusCode = (int)HttpStatusCode.BadRequest;
        response.ContentType = "application/json";

        if (!string.IsNullOrEmpty(message))
        {
            await response.WriteAsync($"{{ \"message\": \"{message}\" }}");
        }
    }
}
