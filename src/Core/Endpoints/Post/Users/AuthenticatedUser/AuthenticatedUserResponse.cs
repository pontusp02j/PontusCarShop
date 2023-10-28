using Core.Domain.Enums.Users;

namespace Core.Endpoints.Post.Users.AuthenticatedUser
{
    public class AuthenticatedUserResponse
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserPermissionLevel PermissionLevel { get; set; }
    }
}
