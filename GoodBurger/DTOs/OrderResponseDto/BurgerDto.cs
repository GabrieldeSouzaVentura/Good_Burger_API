using System.Text.Json.Serialization;

namespace GoodBurger.DTOs.OrderResponseDto;

public class BurgerDto
{
    public int BurgerId { get; set; }
    public string BurgerName { get; set; }
    [JsonIgnore]
    public double BurgerPrice { get; set; }
    public string burgerPirce => BurgerPrice.ToString("F2");
}
