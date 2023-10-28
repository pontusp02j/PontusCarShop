using Core.Commands.Cars;
using Core.Repositories.Cars;
using Core.Repositories.Users;
using MediatR;

namespace Core.RequestHandlers.Cars
{
    public class CreateCarUserRelationRequestHandler : IRequestHandler<CreateCarUserRelationCommand>
    {
        private readonly ICarRepository _carRepository;
        private readonly IUserRepository _userRepository;

        public CreateCarUserRelationRequestHandler(ICarRepository carRepository, IUserRepository userRepository)
        {
            _carRepository = carRepository;
            _userRepository = userRepository;
        }

        public async Task Handle(CreateCarUserRelationCommand req, CancellationToken cancellationToken)
        {
            try
            {
                var carDto = await _carRepository.GetCarByIdAsync(req.ReqData.ViewedCarsId);

                await _userRepository.AddCarUserRelationAsync(req.ReqData.UsersId, req.ReqData.ViewedCarsId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
