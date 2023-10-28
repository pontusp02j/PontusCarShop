using AutoMapper;
using Core.Commands.Cars;
using Core.Queues;
using Core.Repositories.Cars;
using Core.Responses.Cars;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Shop.Core.Domain.Enums;

namespace Core.RequestHandlers.Cars
{
    public class UpdateCarRequestHandler : IRequestHandler<UpdateCarCommand, CarResponse>
    {
        private readonly IMapper _mapper;
        private readonly ICarRepository _carRepository;
        private readonly INewCarsForSaleQueue _newCarsForSaleQueue;

        public UpdateCarRequestHandler(IMapper mapper, ICarRepository carRepository, INewCarsForSaleQueue newCarsForSaleQueue)
        {
            _mapper = mapper;
            _carRepository = carRepository;
            _newCarsForSaleQueue = newCarsForSaleQueue;
        }
        public async Task<CarResponse> Handle(UpdateCarCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var updatedCarDto = _mapper.Map<CarDto>(request.RequestData);

                var existingCar = await _carRepository.GetCarByIdAsync(updatedCarDto.Id);

                await _carRepository.UpdateAndSaveCarAsync(updatedCarDto);

                await _carRepository.SaveChangesAsync();

                if((existingCar.Status.Equals(Status.Sold) || existingCar.Status.Equals(Status.Unknown)) && updatedCarDto.Status.Equals(Status.Sale))
                {
                    await _newCarsForSaleQueue.WriteAsync(updatedCarDto.ModelName);
                }

                return _mapper.Map<CarResponse>(updatedCarDto);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
