using Core.Responses.Users;
using MediatR;

namespace Core.Queries.Users
{
    public record GetUserByIdQuery(int UserId) : IRequest<UserResponse>;
}
