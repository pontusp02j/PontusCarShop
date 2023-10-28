using Core.Responses.Users;
using MediatR;

namespace Core.Queries.Users
{
    public record GetAllUsersQuery() : IRequest<List<UserResponse>>;
}
