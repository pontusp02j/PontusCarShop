using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Security
{
    public class RestrictedIpAddress
    {
        public int Id { get; set; }
        [MaxLength(40)]
        public string IpAddress { get; set; } = string.Empty;
        public DateTime RestrictionExpiresUtc { get; set; } = DateTime.UtcNow.AddMinutes(10);
    }
}
