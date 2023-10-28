namespace Core.Endpoints.Post.Users.AuthenticatedUser
{
    public class AuthenticatedUserRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
