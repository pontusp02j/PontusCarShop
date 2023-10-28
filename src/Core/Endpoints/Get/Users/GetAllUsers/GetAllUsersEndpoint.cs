using Core.Queries.Users;
using Core.Utilities;
using FastEndpoints;
using MediatR;

namespace Core.Endpoints.Get.Users.GetAllUsers
{
    public class GetAllUsersEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;
        public GetAllUsersEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Get("api/users");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var users = await _mediator.Send(new GetAllUsersQuery());
                await SendAsync(users, StatusCodes.Status200OK, ct);
            }
            catch (Exception)
            {
                await CustomRequestResponses.BadRequestAsync(HttpContext, "Could not get users");
            }
        }
    }
}
