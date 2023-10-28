using AutoMapper;
using Core.Commands.Users;
using Core.Dtos.Users;
using Core.Repositories.Users;
using Core.Responses.Users;
using Core.Services;
using Core.Utilities;
using MediatR;

namespace Core.RequestHandlers.Users
{
    public class CreateUserRequestHandler : IRequestHandler<CreateUserCommand, UserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        public CreateUserRequestHandler(IUserRepository userRepository, IMapper mapper, IEmailService emailService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _emailService = emailService;
        }
        public async Task<UserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userDto = _mapper.Map<UserDto>(request.UserRequest);

                if (userDto.SubscribeToNewCars)
                {
                    userDto.LastSubscribed = DateTime.UtcNow;
                }

                userDto.Password = AuthenticationHandler.HashPassword(request.UserRequest.Password);

                var result = await _userRepository.AddUserAsync(userDto);

                if (result.SubscribeToNewCars)
                {
                    await _emailService.SendEmailAsync("PontusAndEric@TheCarShop.com", result.Email, "Email Verification", EmailMessages.VerificationEmail(result.FirstName, request.VerificationUrl));
                }

                return _mapper.Map<UserResponse>(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
