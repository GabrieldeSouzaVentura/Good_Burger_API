using GoodBurger.Validations;
using System.Text.Json.Serialization;

namespace GoodBurger.Models;

public class Order
{
    public int OrderId { get; set; }
    [BurgerValidation]
    public int BurgerId { get; set; }
    public Burger Burger { get; set; }
    [ExtraValidation]
    public List<Extra> Extras { get; set; } = new();
    [JsonIgnore]
    public double Total { get; set; }
    public string total => Total.ToString("F2");
    [JsonIgnore]
    public double Discount { get; set; }
    public string discount => Discount.ToString("F2");

}
