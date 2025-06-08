using GoodBurger.Context;
using GoodBurger.Repositories.IRepositories;

namespace GoodBurger.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private IBurgerRepository _burgerRepo;
    private IExtraRepository _extraRepo;
    private IOrderRepository _orderRepo;

    public AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IOrderRepository OrderRepository
    {
        get { return _orderRepo = _orderRepo ?? new OrderRepository(_context); }
    }

    public IBurgerRepository BurgerRepository
    {
        get { return _burgerRepo = _burgerRepo ?? new BurgerRepository(_context); }
    }

    public IExtraRepository ExtraRepository
    {
        get { return _extraRepo = _extraRepo ?? new ExtraRepository(_context); }
    }
    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
}
