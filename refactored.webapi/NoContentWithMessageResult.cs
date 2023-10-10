using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

public class NoContentWithMessageResult : IActionResult
{
    private readonly string message;

    public NoContentWithMessageResult(string message)
    {
        this.message = message;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;
        response.StatusCode = (int)HttpStatusCode.NoContent;
        response.ContentType = "application/json";

        if (!string.IsNullOrEmpty(message))
        {
            await response.WriteAsync($"{{ \"message\": \"{message}\" }}");
        }
    }
}
