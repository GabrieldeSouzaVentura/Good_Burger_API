using GoodBurger.Context;
using GoodBurger.DTOs;
using GoodBurger.Models;
using GoodBurger.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GoodBurger.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public AppDbContext _context;

    public OrderRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Order> CreateOrder(MyOrderDto dto)
    {
        var burger = await _context.Burgers.FindAsync(dto.BurgerId);
        var extra = await _context.Extras.Where(e => dto.ExtrasIds.Contains(e.ExtraId)).ToListAsync();

        if (burger is null || extra.Count != dto.ExtrasIds.Count) throw new Exception("Burger or Extras Invalid");

        double total = burger.Price + extra.Sum(e => e.Price);
        double discount = CalculateDiscount(burger, extra);

        var order = new Order
        {
            Burger = burger,
            Extras = extra,
            Total = total - discount,
            Discount = discount
        };

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        return order;
    }

    public async Task<IEnumerable<Order>> GetOrders()
    {
        return await _context.Orders.Include(o => o.Burger).Include(o => o.Extras).ToListAsync();
    }

    public async Task<Order> OrderUpdate(int Id, MyOrderDto dto)
    {
        var order = await _context.Orders.Include(o => o.Extras).FirstOrDefaultAsync(o => o.OrderId == Id);
        var burger = await _context.Burgers.FindAsync(dto.BurgerId);
        var extra = await _context.Extras.Where(e => dto.ExtrasIds.Contains(e.ExtraId)).ToListAsync();

        if (burger is null || extra.Count != dto.ExtrasIds.Count || order is null) throw new Exception("Burger or Extras Invalid");

        double total = burger.Price + extra.Sum(e => e.Price);
        double discount = CalculateDiscount(burger, extra);

        order.Extras.Clear();
        await _context.SaveChangesAsync();

        order.BurgerId = dto.BurgerId;
        order.Burger = burger;
        order.Total = total - discount;
        order.Discount = discount;
        foreach (var extras in extra) { order.Extras.Add(extras); }

        await _context.SaveChangesAsync();
        return order;
    }

    public double CalculateDiscount(Burger burger, List<Extra> extra)
    {
        double total = burger.Price + extra.Sum(e => e.Price);

        bool HasFries = extra.Any(e => e.Name.ToLower().Contains("french fries"));
        bool HasSoda = extra.Any(e => e.Name.ToLower().Contains("soda"));

        if (HasFries && HasSoda) return total * 0.20;
        if (HasFries) return total * 0.15;
        if (HasSoda) return total * 0.10;

        return 0;
    }
}
