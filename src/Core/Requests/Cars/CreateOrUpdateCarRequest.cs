using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Shop.Core.Domain.Enums;

namespace Core.Requests.Cars
{
    public class CreateOrUpdateCarRequest
    {
        public int Id { get; set; }
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
        public DateTime ModelYear { get; set; }
        public DateTime NextServingDate { get; set; } = DateTime.MinValue;
        public DateTime LastServingDate { get; set; } = DateTime.MinValue;
        public Status Status { get; set; }
        public int Doors { get; set; } = 0;
        public int NumberOfViews { get; set; }
    }
}
