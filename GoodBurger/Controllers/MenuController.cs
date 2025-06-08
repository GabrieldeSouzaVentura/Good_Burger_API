using GoodBurger.Filters;
using GoodBurger.Models;
using GoodBurger.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoodBurger.Controllers;

[ApiController]
[Route("[controller]")]
public class MenuController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<MenuController> _logger;

    public MenuController(IUnitOfWork unitOfWork, ILogger<MenuController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    [Authorize(Policy = "UserOnly")]
    [HttpGet]
    [ServiceFilter(typeof(GoodBurgerLoggingFilters))]
    [ServiceFilter(typeof(GoodBurgerExceptionFilter))]
    public async Task<ActionResult<IEnumerable<Menu>>> GetMenu()
    {
        var burgers = await _unitOfWork.BurgerRepository.GetAllAsync();
        var extras = await _unitOfWork.ExtraRepository.GetAllAsync();

        _logger.LogInformation("----- Get menu -----");
        return new List<Menu>
        {
            new Menu
            {
                Burgers = burgers,
                Extras = extras
            }
        };
    }
}
