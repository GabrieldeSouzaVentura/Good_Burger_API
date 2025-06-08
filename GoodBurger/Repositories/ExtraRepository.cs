using GoodBurger.Context;
using GoodBurger.Models;
using GoodBurger.Repositories.IRepositories;

namespace GoodBurger.Repositories;

public class ExtraRepository : Repository<Extra>, IExtraRepository
{
    public ExtraRepository(AppDbContext context) : base(context)
    {
    }
}
