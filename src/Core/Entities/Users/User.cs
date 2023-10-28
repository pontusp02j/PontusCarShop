using Core.Entities.Cars;
using Core.Entities.Subscriptions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Users
{
    public class User
    {
        public User()
        {
            ViewedCars = new List<Car>();
        }
        public int Id { get; set; }
        [MaxLength(255)]
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        [MaxLength(255)]
        public string Email { get; set; } = "";
        public bool EmailVerified { get; set; }
        [MaxLength(255)]
        public string FirstName { get; set; } = "";
        [MaxLength(255)]
        public string LastName { get; set; } = "";
        [MaxLength(255)]
        public string SwedishRegion { get; set; } = "";
        [MaxLength(255)]
        public string MobilePhoneNumber { get; set; } = "";
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public bool SubscribeToNewCars { get; set; }
        public DateTime LastNotified { get; set; } = DateTime.MinValue.ToUniversalTime();
        public DateTime LastSubscribed { get; set; } = DateTime.MinValue.ToUniversalTime();
        [ForeignKey("UserRole")]
        public int? UserRoleId { get; set; }
        public UserRole? Role { get; set; }
        public Subscription? Subscription { get; set; }
        [ForeignKey("Subscription")]
        public int? SubscriptionId { get; set; }
        public List<Car> ViewedCars { get; set; }
    }
}
