using System.ComponentModel.DataAnnotations;

namespace Core.Endpoints.Post.Users.AuthenticatedUser
{
    public class FailedLoginAttemptDto
    {
        public int Id { get; set; }
        [MaxLength(255)]
        public string Ip { get; set; } = string.Empty;
        public DateTime AttemptTime { get; set; } = DateTime.UtcNow;
    }
}
