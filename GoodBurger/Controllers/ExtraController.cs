using GoodBurger.Filters;
using GoodBurger.Models;
using GoodBurger.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoodBurger.Controllers;

[Route("[controller]")]
[ApiController]
public class ExtraController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ExtraController> _logger;

    public ExtraController(IUnitOfWork unitOfWork, ILogger<ExtraController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    [Authorize(Policy = "UserOnly")]
    [HttpGet]
    [ServiceFilter(typeof(GoodBurgerLoggingFilters))]
    [ServiceFilter(typeof(GoodBurgerExceptionFilter))]
    public async Task<ActionResult<IEnumerable<Extra>>> GetExtra()
    {
        _logger.LogInformation("----- Get extras -----");
        var extras = await _unitOfWork.ExtraRepository.GetAllAsync();

        return Ok(extras);
    }
}
