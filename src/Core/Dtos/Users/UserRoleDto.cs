using Core.Domain.Enums.Users;
using Core.Entities.Users;

namespace Core.Dtos.Users
{
    public class UserRoleDto
    {
        public int Id { get; set; }
        public UserPermissionLevel PermissionLevel { get; set; }
    }
}
