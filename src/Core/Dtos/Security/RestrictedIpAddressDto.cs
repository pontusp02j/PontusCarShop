using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Security
{
    public class RestrictedIpAddressDto
    {
        public int Id { get; set; }
        [MaxLength(40)]
        public string IpAddress { get; set; } = string.Empty;
        public DateTime RestrictionExpiresUtc { get; set; } = DateTime.UtcNow.AddMinutes(10);
    }
}
