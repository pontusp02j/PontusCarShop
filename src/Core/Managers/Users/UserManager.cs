using Core.Repositories.Users;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Core.Managers.Users
{
    public interface IUserManager
    {
        Task<bool> AuthenticateUser(string username, string password);
    }
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<bool> AuthenticateUser(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return false;
            }

            return AuthenticationHandler.VerifyPassword(password, user.Password);
        }
    }
}
