using AutoMapper;
using Core.Commands.Users;
using Core.Repositories.Users;
using Core.Utilities;
using MediatR;
using Core.Endpoints.Put.Users.UpdateUserPasswordEndpoint;
using Core.BackgroundServices.Notifications;

namespace Core.RequestHandlers.Users
{
    public class UpdateUserPasswordRequestHandler : IRequestHandler<UpdateUserPasswordCommand, UpdateUserPasswordResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificationService> _logger;
        public UpdateUserPasswordRequestHandler(IUserRepository userRepository, IMapper mapper,
        ILogger<NotificationService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<UpdateUserPasswordResponse> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(request.RequestData.Id);
                if (user != null)
                {
                    user.Password = AuthenticationHandler.HashPassword(request.RequestData.Password);

                    var result = await _userRepository.UpdateUserAsync(user);

                    return _mapper.Map<UpdateUserPasswordResponse>(result);
                }
                else
                {
                    _logger.LogError("Couldn't find the user: " + request.RequestData.Id);
                    throw new Exception("Error: Couldn't find the user");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong when trying to update user");
                throw new Exception("Error: Something went wrong here");
            }
        }
    }
}
