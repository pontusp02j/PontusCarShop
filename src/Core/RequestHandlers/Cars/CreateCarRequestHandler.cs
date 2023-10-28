using AutoMapper;
using Core.Commands.Cars;
using Core.Queues;
using Core.Repositories.Cars;
using Core.Responses.Cars;
using MediatR;
using Shop.Core.Domain.Enums;

namespace Core.RequestHandlers.Cars
{
    public class CreateCarRequestHandler : IRequestHandler<CreateCarCommand, CarResponse>
    {
        private readonly IMapper _mapper;
        private readonly ICarRepository _carRepository;
        private readonly INewCarsForSaleQueue _newCarsForSaleQueue;

        public CreateCarRequestHandler(IMapper mapper, ICarRepository carRepository, INewCarsForSaleQueue newCarsForSaleQueue)
        {
            _mapper = mapper;
            _carRepository = carRepository;
            _newCarsForSaleQueue = newCarsForSaleQueue;
        }

        public async Task<CarResponse> Handle(CreateCarCommand req, CancellationToken cancellationToken)
        {
            try
            {
                var carDto = await _carRepository.CreateAndSaveCarAsync(_mapper.Map<CarDto>(req.RequestData));

                if (carDto.Status.Equals(Status.Sale))
                {
                    await _newCarsForSaleQueue.WriteAsync(carDto.ModelName);
                }

                var response = _mapper.Map<CarResponse>(carDto);

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
