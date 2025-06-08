using GoodBurger.Filters;
using GoodBurger.Models;
using GoodBurger.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoodBurger.Controllers;

[Route("[controller]")]
[ApiController]
public class BurgerController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BurgerController> _logger;

    public BurgerController(IUnitOfWork unitOfWork, ILogger<BurgerController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    [Authorize(Policy = "UserOnly")]
    [HttpGet]
    [ServiceFilter(typeof(GoodBurgerLoggingFilters))]
    [ServiceFilter(typeof(GoodBurgerExceptionFilter))]
    public async Task<ActionResult<IEnumerable<Burger>>> GetBurger()
    {
        _logger.LogInformation("----- Get burgers -----");
        var burgers = await _unitOfWork.BurgerRepository.GetAllAsync();

        return Ok(burgers);
    }
}
