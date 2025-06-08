using GoodBurger.DTOs.JwtDtos;
using GoodBurger.Filters;
using GoodBurger.Models;
using GoodBurger.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GoodBurger.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(ITokenService tokenService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<AuthController> logger)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _logger = logger;
    }

    [Authorize(Policy = "ExclusiveOnly")]
    [HttpPost]
    [Route("CreateRole")]
    [ServiceFilter(typeof(GoodBurgerLoggingFilters))]
    [ServiceFilter(typeof(GoodBurgerExceptionFilter))]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        var roleExist = await _roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            _logger.LogInformation("----- CreateRole role no exist ----");
            var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));

            if (roleResult.Succeeded)
            {
                _logger.LogInformation("----- CreateRole status 200 success -----");
                return StatusCode(StatusCodes.Status200OK, new ResponseDto { Status = "Success", Message = $"Role {roleName} added successfully" });
            }
            else
            {
                _logger.LogInformation("----- CreateRole status 400 BadRequest -----");
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto { Status = "Error", Message = $"Issue adding the new {roleName} role" });
            }
        }
        _logger.LogInformation("----- CreateRole status 500 error role already exist -----");
        return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto { Status = "Error", Message = "Role already exist" });
    }

    [Authorize(Policy = "ExclusiveOnly")]
    [HttpPost]
    [Route("AddToRole")]
    [ServiceFilter(typeof(GoodBurgerLoggingFilters))]
    [ServiceFilter(typeof(GoodBurgerExceptionFilter))]
    public async Task<IActionResult> AddUserToRole(string email, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user != null)
        {
            _logger.LogInformation("----- AddToRole user exist -----");
            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                _logger.LogInformation("----- AddToRole stautus 200 success -----");
                return StatusCode(StatusCodes.Status200OK, new ResponseDto { Status = "Success", Message = $"User {user.Email} added to the {roleName} role" });
            }
            else
            {
                _logger.LogInformation("----- AddToRole status 400 BadRequest -----");
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                { Status = "Error", Message = $"Error: Unable to add user {user.Email} to the {roleName} role" });
            }
        }
        _logger.LogInformation("----- AddToRole BadRequest no exist -----");
        return BadRequest(new { error = "Unable to find user" });
    }

    [HttpPost]
    [Route("Login")]
    [ServiceFilter(typeof(GoodBurgerLoggingFilters))]
    [ServiceFilter(typeof(GoodBurgerExceptionFilter))]
    public async Task<IActionResult> Login([FromBody] LoginModelDto loginModelDto)
    {
        var user = await _userManager.FindByNameAsync(loginModelDto.UserName!);

        if (user is not null && await _userManager.CheckPasswordAsync(user, loginModelDto.Password!))
        {
            _logger.LogInformation("----- Login is valid -----");
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim("Id", user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = _tokenService.GenerateAccessToken(authClaims, _configuration);

            var refreshToken = _tokenService.GenerateRefreshToken();

            _ = int.TryParse(_configuration["Jwt:RefreshTokenValidityInMinutes"], out int refhreshTojenValidityInMinutes);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refhreshTojenValidityInMinutes);

            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            });
        }
        _logger.LogInformation("----- Login no authorized -----");
        return Unauthorized();
    }

    [HttpPost]
    [Route("Register")]
    [ServiceFilter(typeof(GoodBurgerLoggingFilters))]
    [ServiceFilter(typeof(GoodBurgerExceptionFilter))]
    public async Task<IActionResult> Register([FromBody] RegisterModelDto registerModelDto)
    {
        var userExists = await _userManager.FindByNameAsync(registerModelDto.UserName!);
        var emailExists = await _userManager.FindByEmailAsync(registerModelDto.Email!);

        if (userExists != null || emailExists != null)
        {
            _logger.LogInformation("----- Register status 500 error user or email already exists -----");
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Status = "Error", Message = "User or email already exists" });
        }

        _logger.LogInformation("----- Register valid -----");
        ApplicationUser user = new()
        {
            Email = registerModelDto.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = registerModelDto?.UserName
        };
        var result = await _userManager.CreateAsync(user, registerModelDto.Password!);

        if (!result.Succeeded)
        {
            _logger.LogInformation("----- Register status 500 error user creation failed -----");
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Status = "Error", Message = "User creation failed" });
        }
        _logger.LogInformation("----- Register status 200 user created successfully -----");
        return Ok(new ResponseDto { Status = "Success", Message = "User created successfully" });
    }

    [HttpPost]
    [Route("Refresh-Token")]
    [ServiceFilter(typeof(GoodBurgerLoggingFilters))]
    [ServiceFilter(typeof(GoodBurgerExceptionFilter))]
    public async Task<IActionResult> RefreshToken(TokenModelDto tokenModelDto)
    {
        if (tokenModelDto is null) return BadRequest("Invalid client request"); _logger.LogInformation("----- Refresh-Token badrequest -----");

        string? accessToken = tokenModelDto.AccessToken ?? throw new ArgumentNullException(nameof(tokenModelDto));
        string? refreshToken = tokenModelDto.RefreshToken ?? throw new ArgumentNullException(nameof(tokenModelDto));

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);

        if (principal == null) return BadRequest("Invalid access token/refresh token"); _logger.LogInformation("----- Refresh-Token badrequest -----");

        string username = principal.Identity.Name;

        var user = await _userManager.FindByNameAsync(username!);

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            _logger.LogInformation("----- Refresh-Token BadRequest invalid access token/refresh token -----");
            return BadRequest("Invalid access token/refresh token");
        }
        _logger.LogInformation("----- Refresh-Token BadRequest access token/refresh token -----");

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);

        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return new ObjectResult(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken), refreshToken = newRefreshToken
        });
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    [Route("Revoke/{username}")]
    [ServiceFilter(typeof(GoodBurgerLoggingFilters))]
    [ServiceFilter(typeof(GoodBurgerExceptionFilter))]
    public async Task<IActionResult> Revoke(string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user == null) return BadRequest("Invalid user name"); _logger.LogInformation("----- Revoke BadRequest-----");

        _logger.LogInformation("----- Revoke valid -----");
        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);

        return NoContent();
    }
}