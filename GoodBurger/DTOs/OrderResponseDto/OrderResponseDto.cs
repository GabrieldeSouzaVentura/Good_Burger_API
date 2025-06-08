using System.Text.Json.Serialization;

namespace GoodBurger.DTOs.OrderResponseDto;

public class OrderResponseDto
{
    public int OrderId { get; set; }
    public BurgerDto Burger { get; set; }
    public List<ExtraDto> Extras { get; set; } = new();
    [JsonIgnore]
    public double Total { get; set; }
    public string total => Total.ToString("F2");
    [JsonIgnore]
    public double Discount { get; set; }
    public string discount => Discount.ToString("F2");
}
