using Core.Queries.Users;
using Core.Responses.Users;
using Core.Utilities;
using FastEndpoints;
using MediatR;

namespace Core.Endpoints.Get.Users.GetUserById
{
    public class GetUserByIdEndpoint : EndpointWithoutRequest<UserResponse>
    {
        private readonly IMediator _mediator;

        public GetUserByIdEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override void Configure()
        {
            Get("api/users/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                var userId = Route<int>("id");
                var user = await _mediator.Send(new GetUserByIdQuery(userId));

                await SendAsync(user, StatusCodes.Status200OK, ct);
            }
            catch (Exception)
            {
                await CustomRequestResponses.BadRequestAsync(HttpContext, "Something went wrong getting user");
            }
        }
    }
}
