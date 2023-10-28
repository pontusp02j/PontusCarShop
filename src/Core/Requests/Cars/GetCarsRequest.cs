using Shop.Core.Domain.Enums;

namespace Core.Requests.Cars
{
    public class GetCarsRequest
    {
        public int Id { get; set; } = -1;
        public string? Type { get; set; }
        public string? Description { get; set; }
        public Status Status { get; set; }
    }
}
