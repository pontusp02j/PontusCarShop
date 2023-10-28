using Core.Domain.Enums.Subscriptions;

namespace Core.Requests.Subscriptions
{
    public class SubscriptionRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public SubscriptionType SubscriptionType { get; set; }
        public NotificationInterval NotificationInterval { get; set; }
        public bool IsActive { get; set; }
    }
}
