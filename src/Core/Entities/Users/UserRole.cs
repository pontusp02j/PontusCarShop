using Core.Domain.Enums.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Users
{
    public class UserRole
    {
        public UserRole()
        {
            Users = new List<User>();
        }
        public int Id { get; set; }
        public List<User> Users { get; set; }
        public UserPermissionLevel PermissionLevel { get; set; }

    }
}
