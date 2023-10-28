using AutoMapper;
using Core.Queries;
using Core.Queries.Cars;
using Core.Repositories.Cars;
using Core.Requests.Cars;
using Core.Responses.Cars;
using MediatR;
using Shop.Core.Domain.Enums;

namespace Core.RequestHandlers.Cars
{
    public class GetAllCarsRequestHandler : IRequestHandler<GetAllCarsQuery, List<CarResponse>>
    {
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;

        public GetAllCarsRequestHandler(ICarRepository carRepository, IMapper mapper)
        {
            _carRepository = carRepository;
            _mapper = mapper;
        }

        public async Task<List<CarResponse>> Handle(GetAllCarsQuery req, CancellationToken cancellationToken)
        {
            try
            {
                var query = (await _carRepository.GetAllCarsAsync()).AsQueryable();

                if (!string.IsNullOrWhiteSpace(req.RequestData.Description) || !string.IsNullOrWhiteSpace(req.RequestData.Type))
                {
                    query = FetchCarsFromDescriptionAndType(query, req.RequestData);
                }

                if (!string.IsNullOrWhiteSpace(req.RequestData.Type) && string.IsNullOrWhiteSpace(req.RequestData.Description))
                {
                    query = query.Where(c => c.Type.Contains(req.RequestData.Type));
                }

                else if (!string.IsNullOrWhiteSpace(req.RequestData.Type) && !string.IsNullOrWhiteSpace(req.RequestData.Description))
                {
                    query = FetchCarsFromDescriptionAndType(query, req.RequestData);
                }

                if (req.RequestData.Status == Status.Sold || req.RequestData.Status == Status.Sale)
                {
                    query = query.Where(c => c.Status == req.RequestData.Status);
                }

                if (req.RequestData.Id != -1)
                {
                    query = query.Where(c => c.Id == req.RequestData.Id);
                }

                var cars = query.ToList();

                var response = cars.Select(_ => _mapper.Map<CarResponse>(_)).ToList();

                return response;

            }
            catch (Exception)
            {
                throw;
            }
        }

        private IQueryable<CarDto> FetchCarsFromDescriptionAndType(IQueryable<CarDto> query, GetCarsRequest option)
        {
            return query
                    .Where(c =>
                        string.IsNullOrWhiteSpace(option.Description) || c.Description.Contains(option.Description)
                        ||
                        string.IsNullOrWhiteSpace(option.Type) || c.Type.Contains(option.Type));

        }
    }
}
