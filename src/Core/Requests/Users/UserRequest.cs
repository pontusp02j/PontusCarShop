namespace Core.Requests.Users
{
    public class UserRequest
    {
        public int Id { get; set; }
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";

        public string SwedishRegion { get; set; } = "";
        public string MobilePhoneNumber { get; set; } = "";
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public bool SubscribeToNewCars { get; set; }
        public DateTime LastNotified { get; set; } = DateTime.MinValue.ToUniversalTime();
        public DateTime LastSubscribed { get; set; } = DateTime.MinValue.ToUniversalTime();
        public int? SubscriptionId { get; set; }
    }
}
