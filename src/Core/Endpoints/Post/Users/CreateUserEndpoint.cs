using Core.Commands.Users;
using Core.Requests.Users;
using Core.Responses.Users;
using Core.Utilities;
using FastEndpoints;
using MediatR;
using System.Net;

namespace Core.Endpoints.Post.Users
{
    public class CreateUserEndpoint : Endpoint<UserRequest, UserResponse>
    {
        private readonly IMediator _mediator;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public CreateUserEndpoint(IMediator mediator, JwtTokenGenerator jwtTokenGenerator)
        {
            _mediator = mediator;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public override void Configure()
        {
            Post("/api/users/create");
            AllowAnonymous();
        }

        public override async Task HandleAsync(UserRequest req, CancellationToken ct)
        {
            try
            {
                var token = _jwtTokenGenerator.GenerateToken(req.UserName);
                var verificationUrl = $"{HttpHelper.GetBaseUrl(HttpContext)}/api/users/verifyEmail?token={token}";
                var response = await _mediator.Send(new CreateUserCommand(req, verificationUrl));
                await SendAsync(response, StatusCodes.Status200OK, ct);
            }
            catch (Exception)
            {
                await BadRequestAsync("Something went wrong when creating user");
            }
        }
        private async Task BadRequestAsync(string message)
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(message)
            };

            await HttpContext.Response.WriteAsync(await response.Content.ReadAsStringAsync());
        }
    }
}
