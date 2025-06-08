using GoodBurger.DTOs;
using GoodBurger.Models;

namespace GoodBurger.Repositories.IRepositories;

public interface IOrderRepository : IRepository<Order>
{
    public Task <IEnumerable<Order>> GetOrders();
    public Task <Order> OrderUpdate(int Id, MyOrderDto dto);
    public Task <Order> CreateOrder(MyOrderDto dto);
}
