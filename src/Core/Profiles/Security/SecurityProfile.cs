using AutoMapper;
using Core.Dtos.Security;
using Core.Endpoints.Post.Users.AuthenticatedUser;
using Core.Entities.Security;
using Core.Entities.Users;

namespace Core.Profiles.Security
{
    public class SecurityProfile : Profile
    {
        public SecurityProfile()
        {
            CreateMap<FailedLoginAttempt, FailedLoginAttemptDto>();
            CreateMap<FailedLoginAttemptDto, FailedLoginAttempt>();

            CreateMap<RestrictedIpAddress, RestrictedIpAddressDto>();
            CreateMap<RestrictedIpAddressDto, RestrictedIpAddress>();
        }
    }
}
