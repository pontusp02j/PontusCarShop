using System.Net;

namespace Core.Utilities
{
    public static class CustomRequestResponses
    {
        public static async Task BadRequestAsync(HttpContext httpContext, string message)
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(message)
            };

            await httpContext.Response.WriteAsync(await response.Content.ReadAsStringAsync());
        }
    }
}
