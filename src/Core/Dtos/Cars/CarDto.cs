using Core.Dtos.Users;
using Shop.Core.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

public class CarDto
{
    public CarDto()
    {
        Users = new List<UserDto>();
    }
    public int Id { get; set; }
    public List<UserDto> Users { get; set; }
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
    public DateTime ModelYear { get; set; } = DateTime.MinValue;
    public DateTime NextServingDate { get; set; } = DateTime.MinValue;
    public DateTime LastServingDate { get; set; } = DateTime.MinValue;
    public Status Status { get; set; }
    public int Doors { get; set; } = 0;
    public int NumberOfViews { get; set; }
}
