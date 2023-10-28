using Core.Requests.Cars;
using Core.Responses.Cars;
using MediatR;

namespace Core.Commands.Cars
{
    public record CreateCarCommand(CreateOrUpdateCarRequest RequestData) : IRequest<CarResponse>;
}
