using Core.Commands.Users;
using Core.Repositories.Users;
using Core.Requests.Users;
using Core.Responses.Users;
using Core.Utilities;
using FastEndpoints;
using MediatR;

namespace Core.Endpoints.Put.Users.UpdateUser
{
    public class UpdateUserEndpoint : Endpoint<UpdateUserRequest, UserResponse>
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;

        public UpdateUserEndpoint(IMediator mediator, IUserRepository userRepository)
        {
            _mediator = mediator;
            _userRepository = userRepository;
        }
        public override void Configure()
        {
            Put("api/users/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(UpdateUserRequest req, CancellationToken ct)
        {
            try
            {
                var userId = Route<int>("id");
                var existingUserDto = await _userRepository.GetUserByIdAsync(userId);

                if(existingUserDto == null)
                {
                    AddError(_ => _.Id, "No user was found for this id");
                }

                ThrowIfAnyErrors();

                var updatedUser = await _mediator.Send(new UpdateUserCommand(req, existingUserDto!), ct);

                await SendAsync(updatedUser, StatusCodes.Status200OK, ct);
            }
            catch (Exception)
            {
                await CustomRequestResponses.BadRequestAsync(HttpContext, "Something went wrong getting user");
            }
        }
    }
}
