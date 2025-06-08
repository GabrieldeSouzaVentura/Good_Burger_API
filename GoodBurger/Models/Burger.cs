using System.Text.Json.Serialization;

namespace GoodBurger.Models;

public class Burger
{
    public int BurgerId { get; set; }
    public string Name { get; set; }
    [JsonIgnore]
    public double Price { get; set; }
    public string price => Price.ToString("F2");
}
