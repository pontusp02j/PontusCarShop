using AutoMapper;
using Core.Commands.Users;
using Core.Repositories.Users;
using MediatR;
using Core.Endpoints.Post.Users.AuthenticatedUser;
using Core.Managers.Users;
using Core.Dtos.Security;
using Core.Entities.Security;
using Core.Entities.Users;

namespace Core.RequestHandlers.Users
{
    public class AuthenticatedUserHandler : IRequestHandler<AuthenticatedUserCommand, AuthenticatedUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ISecurityRepository _securityRepository;
        private readonly IMapper _mapper;
        private readonly IUserManager _userManager;
        public AuthenticatedUserHandler(IUserRepository userRepository, IMapper mapper, IUserManager userManager, ISecurityRepository securityRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userManager = userManager;
            _securityRepository = securityRepository;
        }
        public async Task<AuthenticatedUserResponse> Handle(AuthenticatedUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userManager.AuthenticateUser(request.authenticatedUserRequest.Username, request.authenticatedUserRequest.Password);

                if (result)
                {
                    var user = await _userRepository.GetUserByUsernameAsync(request.authenticatedUserRequest.Username);
                    var response = _mapper.Map<AuthenticatedUserResponse>(user);
                    await _securityRepository.RemoveFailedLoginAttemptsForIpAsync(request.Ip);
                    await _securityRepository.RemoveRestrictedIpAddressesForIpAsync(request.Ip);
                    await _securityRepository.SaveSecurityChangesAsync();

                    return response;
                }
                else
                {
                    if (await _securityRepository.IsLastFailedAttemptBeforeRestrictionForIp(request.Ip))
                    {
                        await _securityRepository.RemoveFailedLoginAttemptsForIpAsync(request.Ip);
                        await _securityRepository.AddRestrictedIpAddressAsync(_mapper.Map<RestrictedIpAddress>(new RestrictedIpAddressDto { IpAddress = request.Ip }));
                    }
                    else
                    {
                        var loginAttemptDto = new FailedLoginAttemptDto
                        {
                            Ip = request.Ip,
                        };

                        await _securityRepository.AddFailedLoginAttemptAsync(_mapper.Map<FailedLoginAttempt>(loginAttemptDto));
                    }

                    await _securityRepository.SaveSecurityChangesAsync();
                    throw new Exception("Authentication failed");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
