using FastEndpoints;
using MediatR;
using Core.Endpoints.Post.Users.AuthenticatedUser;
using Core.Commands.Users;

namespace Core.Endpoints.Get.Users.GetAllUsers
{
    public class AuthenticatedUserEndpoint : Endpoint<AuthenticatedUserRequest, AuthenticatedUserResponse>
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _accessor;
        public AuthenticatedUserEndpoint(IMediator mediator, IHttpContextAccessor accessor)
        {
            _mediator = mediator;
            _accessor = accessor;
        }

        public override void Configure()
        {
            Post("/api/login/user");
            AllowAnonymous();
        }

        public override async Task HandleAsync(AuthenticatedUserRequest req, CancellationToken ct)
        {
            string? userIpAddress = _accessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

            if (string.IsNullOrWhiteSpace(userIpAddress))
            {
                await SendNotFoundAsync();
                return;
            }

            try
            {
                var response = await _mediator.Send(new AuthenticatedUserCommand(req, userIpAddress));
                await SendAsync(response, StatusCodes.Status200OK, ct);
            }
            catch (Exception)
            {
                await SendUnauthorizedAsync();
            }
        }
    }
}
