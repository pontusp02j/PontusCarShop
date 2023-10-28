namespace Core.Requests.Users
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";

        public string SwedishRegion { get; set; } = "";
        public string MobilePhoneNumber { get; set; } = "";
        public bool SubscribeToNewCars { get; set; }
        public int? SubscriptionId { get; set; }
    }
}
