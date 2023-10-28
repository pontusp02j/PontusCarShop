using Core.Requests.Users;
using Core.Responses.Users;
using MediatR;

namespace Core.Commands.Users
{
    public record CreateUserCommand(UserRequest UserRequest, string VerificationUrl) : IRequest<UserResponse>;
}
