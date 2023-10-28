using AutoMapper;
using Core.Queries.Users;
using Core.Repositories.Users;
using Core.Responses.Users;
using MediatR;

namespace Core.RequestHandlers.Users
{
    public class GetUserByIdRequestHandler : IRequestHandler<GetUserByIdQuery, UserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserByIdRequestHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<UserResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(request.UserId);

                return _mapper.Map<UserResponse>(user);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
