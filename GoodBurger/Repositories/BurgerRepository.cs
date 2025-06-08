using GoodBurger.Context;
using GoodBurger.Models;
using GoodBurger.Repositories.IRepositories;

namespace GoodBurger.Repositories;

public class BurgerRepository : Repository<Burger>, IBurgerRepository
{
    public BurgerRepository(AppDbContext context) : base(context)
    {
    }
}
