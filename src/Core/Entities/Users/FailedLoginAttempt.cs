using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Core.Entities.Users
{
    public class FailedLoginAttempt
    {
        public int Id { get; set; }
        [MaxLength(255)]
        public string Ip { get; set; } = string.Empty;
        public DateTime AttemptTime { get; set; } = DateTime.UtcNow; 
    }
}
