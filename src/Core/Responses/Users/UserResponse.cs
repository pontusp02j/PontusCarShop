using Core.Domain.Enums.Users;
using Core.Entities.Cars;
using Core.Entities.Subscriptions;
using Core.Entities.Users;
using Core.Responses.Cars;
using Core.Responses.Subscriptions;

namespace Core.Responses.Users
{
    public class UserResponse
    {
        public UserResponse()
        {
            ViewedCars = new List<CarResponse>();
        }
        public int Id { get; set; }
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string FullName { get; set; } = "";

        public string SwedishRegion { get; set; } = "";
        public string MobilePhoneNumber { get; set; } = "";
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public bool SubscribeToNewCars { get; set; }
        public DateTime LastNotified { get; set; } = DateTime.MinValue.ToUniversalTime();
        public DateTime LastSubscribed { get; set; } = DateTime.MinValue.ToUniversalTime();
        public UserPermissionLevel Role { get; set; }
        public SubscriptionResponse? Subscription { get; set; }
        public int? SubscriptionId { get; set; }
        public List<CarResponse> ViewedCars { get; set; }
    }
}
