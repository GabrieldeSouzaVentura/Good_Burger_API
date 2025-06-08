using System.Text.Json.Serialization;

namespace GoodBurger.DTOs.OrderResponseDto;

public class ExtraDto
{
    public int ExtraId { get; set; }
    public string ExtraName { get; set; }
    [JsonIgnore]
    public double ExtraPrice { get; set; }
    public string extraPrice => ExtraPrice.ToString("F2");
}
