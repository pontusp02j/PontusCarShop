using Core.Requests.Cars;
using Core.Responses.Cars;
using MediatR;

namespace Core.Commands.Cars
{
    public record UpdateCarCommand(CreateOrUpdateCarRequest RequestData) : IRequest<CarResponse>;
}
