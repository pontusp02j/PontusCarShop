using AutoMapper;
using Core.Queries.Users;
using Core.Repositories.Users;
using Core.Responses.Users;
using MediatR;

namespace Core.RequestHandlers.Users
{
    public class GetAllUsersRequestHandler : IRequestHandler<GetAllUsersQuery, List<UserResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public GetAllUsersRequestHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<List<UserResponse>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return (await _userRepository.GetAllUsersAsync()).Select(_ => _mapper.Map<UserResponse>(_)).ToList();
        }
    }
}
