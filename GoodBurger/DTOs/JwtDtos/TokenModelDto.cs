﻿namespace GoodBurger.DTOs.JwtDtos;

public class TokenModelDto
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}
