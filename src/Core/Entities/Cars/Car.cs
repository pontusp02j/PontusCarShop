using Shop.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities.Users;

namespace Core.Entities.Cars
{
    public class Car
    {
        public Car()
        {
            Users = new List<User>();
        }
        public int Id { get; set; }
        public List<User> Users { get; set; }
        [MaxLength(255)]
        public string ModelName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        [MaxLength(600)]
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public double Mileage { get; set; } = 0.0;
        [MaxLength(255)]
        public string Brand { get; set; } = string.Empty;
        public int EngineSize { get; set; } = 0;
        public DateTime ModelYear { get; set; } = DateTime.MinValue.ToUniversalTime();
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public DateTime NextServingDate { get; set; } = DateTime.MinValue.ToUniversalTime();
        public DateTime LastServingDate { get; set; } = DateTime.MinValue.ToUniversalTime();
        public Status Status { get; set; }
        public int Doors { get; set; } = 0;
        public int NumberOfViews { get; set; }
    }
}