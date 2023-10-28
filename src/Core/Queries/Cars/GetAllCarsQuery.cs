using Core.Requests.Cars;
using Core.Responses.Cars;
using MediatR;

namespace Core.Queries.Cars
{
    public record GetAllCarsQuery(GetCarsRequest RequestData) : IRequest<List<CarResponse>>;
}
