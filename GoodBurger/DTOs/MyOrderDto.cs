namespace GoodBurger.DTOs;

public class MyOrderDto
{
    public int BurgerId { get; set; }
    public List<int> ExtrasIds { get; set; }
}
