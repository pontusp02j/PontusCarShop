using FastEndpoints;
using MediatR;
using Core.Commands.Users;
using Core.Utilities;

namespace Core.Endpoints.Put.Users.UpdateUserPasswordEndpoint
{
    public class UpdateUserPasswordEndpoint : Endpoint<UpdateUserPasswordRequest, UpdateUserPasswordResponse>
    {
        private readonly IMediator _mediator;

        public UpdateUserPasswordEndpoint(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Put("/api/users/updatePassword");
            AllowAnonymous();
        }

        public override async Task HandleAsync(UpdateUserPasswordRequest req, CancellationToken ct)
        {
            try
            {
                var response = await _mediator.Send(new UpdateUserPasswordCommand(req));
                await SendAsync(response, StatusCodes.Status200OK, ct);
            }
            catch (Exception)
            {
                await CustomRequestResponses.BadRequestAsync(HttpContext, "Could not update user's password");
            }
        }
    }
}
