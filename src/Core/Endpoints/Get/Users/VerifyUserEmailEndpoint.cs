using Core.Commands.Users;
using FastEndpoints;
using MediatR;

namespace Core.Endpoints.Get.Users
{
  public class VerifyUserEmailEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public VerifyUserEmailEndpoint(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        public override void Configure()
        {
            Get("/api/users/verifyEmail");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var token = Query<string>("token");

                if (string.IsNullOrWhiteSpace(token))
                {
                    throw new ArgumentNullException(nameof(token));
                }

                var emailVerified = await _mediator.Send(new VerifyUserEmailCommand(token));

                if (!emailVerified)
                {
                    throw new Exception("Could not verify email");
                }

                HttpContext.Response.ContentType = "text/html";
                await HttpContext.Response.WriteAsync($"<html><body><script>window.location.href = 'https://localhost:44420/emailConfirmed'</script></body></html>");

                return;
            }
            catch (Exception)
            {
                HttpContext.Response.ContentType = "text/html";
                await HttpContext.Response.WriteAsync($"<html><body><script>window.location.href = 'https://localhost:44420/emailConfirmationFailed'</script></body></html>");
                return;
            }
        }
    }
}
