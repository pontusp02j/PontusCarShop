using Core.Domain.Enums.Users;

namespace Core.Responses.Users
{
    public class UserRoleResponse
    {
        public int Id { get; set; }
        public UserPermissionLevel PermissionLevel { get; set; }
    }
}
