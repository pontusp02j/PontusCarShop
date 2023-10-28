using Core.Domain.Enums.Subscriptions;
using Core.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Subscriptions
{
    public class Subscription
    {
        public Subscription()
        {
            Users = new List<User>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public SubscriptionType SubscriptionType { get; set; }
        public NotificationInterval NotificationInterval { get; set; }
        public bool IsActive { get; set; }
        public List<User> Users { get; set; }
    }
}
