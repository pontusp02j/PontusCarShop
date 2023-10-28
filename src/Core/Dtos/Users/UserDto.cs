using Core.Dtos.Subscriptions;
using Core.Entities.Cars;
using Core.Entities.Subscriptions;
using Core.Entities.Users;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Users
{
    public class UserDto
    {
        public UserDto()
        {
            ViewedCars = new List<CarDto>();
        }
        public int Id { get; set; }
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string Email { get; set; } = "";
        public bool EmailVerified { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";

        public string SwedishRegion { get; set; } = "";
        public string MobilePhoneNumber { get; set; } = "";
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public bool SubscribeToNewCars { get; set; }
        public DateTime LastNotified { get; set; } = DateTime.MinValue.ToUniversalTime();
        public DateTime LastSubscribed { get; set; } = DateTime.MinValue.ToUniversalTime();
        public UserRoleDto? Role { get; set; }
        public int? UserRoleId { get; set; }
        public SubscriptionDto? Subscription { get; set; }
        public int? SubscriptionId { get; set; }
        public List<CarDto> ViewedCars { get; set; }
    }
}
