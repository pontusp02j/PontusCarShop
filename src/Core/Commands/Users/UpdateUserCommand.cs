using Core.Dtos.Users;
using Core.Requests.Users;
using Core.Responses.Users;
using MediatR;

namespace Core.Commands.Users
{
    public record UpdateUserCommand(UpdateUserRequest UserRequest, UserDto ExistingUserDto) : IRequest<UserResponse>;
}
