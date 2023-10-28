using Core.Entities.Users;
using Shop.Core.Services;
using Core.Domain.Enums.Users;

namespace Core.Utilities
{
    public static class InitialSetup
    {
        public async static Task InsertInitialDbData(CarShopDbContext dbContext)
        {
            if (!dbContext.UserRoles.Any())
            {
                dbContext.UserRoles.AddRange(
                    new UserRole
                    {
                        Users = new List<User>(),
                        PermissionLevel = UserPermissionLevel.User
                    },
                    new UserRole
                    {
                        Users = new List<User>(),
                        PermissionLevel = UserPermissionLevel.Admin
                    }
                );
                await dbContext.SaveChangesAsync();
            }
        }
    }

}
