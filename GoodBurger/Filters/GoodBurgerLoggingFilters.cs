using Microsoft.AspNetCore.Mvc.Filters;

namespace GoodBurger.Filters;

public class GoodBurgerLoggingFilters : IActionFilter
{
    private readonly ILogger <GoodBurgerLoggingFilters> _logger;

    public GoodBurgerLoggingFilters(ILogger<GoodBurgerLoggingFilters> logger)
    {
        _logger = logger;
    }
    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("-------------------------------------------");
        _logger.LogInformation("\tOnActionExecuting");
        _logger.LogInformation($"\t{DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"\tSatatus Code : {context.HttpContext.Response.StatusCode}");
        _logger.LogInformation("--------------------------------------------");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("-------------------------------------------");
        _logger.LogInformation("\tOnActionExecuted");
        _logger.LogInformation($"\t{DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"\tModelStade : {context.ModelState.IsValid}");
        _logger.LogInformation("--------------------------------------------");
    }
}