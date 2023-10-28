using AutoMapper;
using Core.Commands.Users;
using Core.Dtos.Users;
using Core.Repositories.Users;
using Core.Responses.Users;
using MediatR;

namespace Core.RequestHandlers.Users
{
    public class UpdateUserRequestHandler : IRequestHandler<UpdateUserCommand, UserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateUserRequestHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<UserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userDto = _mapper.Map<UserDto>(request);

                var updatedUserDto = await _userRepository.UpdateUserAsync(userDto);

                return _mapper.Map<UserResponse>(updatedUserDto);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
