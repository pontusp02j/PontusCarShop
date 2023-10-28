using MediatR;

namespace Core.Commands.Users
{
    public record VerifyUserEmailCommand(string Token) : IRequest<bool>;
}
