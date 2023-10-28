using Core.Endpoints.Put.Users.UpdateUserPasswordEndpoint;
using MediatR;

namespace Core.Commands.Users
{
    public record UpdateUserPasswordCommand(UpdateUserPasswordRequest RequestData) : IRequest<UpdateUserPasswordResponse>;
}
