using Core.Endpoints.Post.Users.AuthenticatedUser;
using MediatR;

namespace Core.Commands.Users
{
    public record AuthenticatedUserCommand(AuthenticatedUserRequest authenticatedUserRequest, string Ip) : IRequest<AuthenticatedUserResponse>;
}
