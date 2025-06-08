namespace GoodBurger.Models;

public class Menu
{
    public IEnumerable<Burger> Burgers { get; set; }
    public IEnumerable<Extra> Extras { get; set; }
}
