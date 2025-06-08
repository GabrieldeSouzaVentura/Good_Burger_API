namespace GoodBurger.Repositories.IRepositories;

public interface IUnitOfWork
{
    IOrderRepository OrderRepository { get; }
    IBurgerRepository BurgerRepository { get; }
    IExtraRepository ExtraRepository { get; }
    Task CommitAsync();
}
