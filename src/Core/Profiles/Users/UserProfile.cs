using AutoMapper;
using Core.Dtos.Users;
using Core.Entities.Users;
using Core.Requests.Users;
using Core.Responses.Users;
using Core.Endpoints.Put.Users.UpdateUserPasswordEndpoint;
using Core.Endpoints.Post.Users.AuthenticatedUser;
using Core.Commands.Users;

namespace Core.Profiles.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dto => dto.Subscription, opt => opt.MapFrom(src => src.Subscription));
            CreateMap<UserDto, User>();
            CreateMap<UserDto, UserResponse>()
                 .ForMember(res => res.Role, opt => opt.MapFrom(src => src.Role!.PermissionLevel))
                 .ForMember(res => res.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
            CreateMap<UserRequest, UserDto>();

            CreateMap<UpdateUserPasswordRequest, UserDto>();
            CreateMap<UserDto, UpdateUserPasswordResponse>();

            CreateMap<UserDto, AuthenticatedUserResponse>()
                      .ForMember(res => res.PermissionLevel, opt => opt.MapFrom(src => src.Role!.PermissionLevel));


            CreateMap<UserRoleDto, UserRole>();
            CreateMap<UserRole, UserRoleDto>();
            CreateMap<UserRoleDto, UserRoleResponse>();

            CreateMap<UpdateUserCommand, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserRequest.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.UserRequest.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.UserRequest.LastName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserRequest.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserRequest.Email))
                .ForMember(dest => dest.SwedishRegion, opt => opt.MapFrom(src => src.UserRequest.SwedishRegion))
                .ForMember(dest => dest.MobilePhoneNumber, opt => opt.MapFrom(src => src.UserRequest.MobilePhoneNumber))
                .ForMember(dest => dest.SubscribeToNewCars, opt => opt.MapFrom(src => src.UserRequest.SubscribeToNewCars))
                .ForMember(dest => dest.SubscriptionId, opt => opt.MapFrom(src => src.UserRequest.SubscriptionId))
                .ForMember(dest => dest.EmailVerified, opt => opt.MapFrom(src => src.ExistingUserDto.EmailVerified))
                .ForMember(dest => dest.LastSubscribed, opt => opt.MapFrom(src => src.ExistingUserDto.LastSubscribed))
                .ForMember(dest => dest.LastNotified, opt => opt.MapFrom(src => src.ExistingUserDto.LastNotified))
                .ForMember(dest => dest.UserRoleId, opt => opt.MapFrom(src => src.ExistingUserDto.UserRoleId))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.ExistingUserDto.Password))
                .ForMember(dest => dest.CreatedUtc, opt => opt.MapFrom(src => src.ExistingUserDto.CreatedUtc));
        }
    }
}
