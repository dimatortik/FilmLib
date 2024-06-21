using FilmLib.API.Contracts;
using FilmLib.API.Contracts.User;
using FilmLib.Application.Auth;
using FilmLib.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmLib.API.Controllers;
[ApiController]
public class UserController(AuthService authService) : ControllerBase
{
    [Route("api/register")]
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUser request, CancellationToken cancellationToken)
    {
        var response = await authService.Register(request.Email, request.Password, request.Username);
        if (response.IsFailure)
        {
            return BadRequest(response.Error);
        }
        
        HttpContext.Response.Cookies.Append("cosy", response.Value);
        
        return Ok();
    }
    
    [Route("api/login")]
    [HttpPost]
    public async Task<IActionResult> LoginUser([FromBody] LoginUser request, CancellationToken cancellationToken)
    {
        var response = await authService.Login(request.Email, request.Password);
        if (response.IsFailure)
        {
            return BadRequest(response.Error);
        }
        HttpContext.Response.Cookies.Append("cosy", response.Value);
        
        return Ok();
    }
    
    [Route("api/user/logout")]
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> LogoutUser()
    {
        return Ok();
    }
    
    [Route("api/change/password")]
    [HttpPut]
    [Authorize(Policy = nameof(Policy.UserPolicy))]
    public async Task<IActionResult> ChangePassword([FromForm] ChangePassword request, CancellationToken cancellationToken)
    {
        string authHeader = Request.Headers["Authorization"];
        if (authHeader == null || !authHeader.StartsWith("Bearer "))
        {
            return BadRequest("Invalid token.");
        }
        var jwt = authHeader.Substring("Bearer ".Length).Trim();
        var userId = authService.GetUserId(jwt);
        if (userId.IsFailure)
        {
            return BadRequest(userId.Error);
        }
        var response = await authService.ChangePassword(userId.Value, request.OldPassword, request.NewPassword);
        if (response.IsFailure)
        {
            return BadRequest(response.Error);
        }
        
        return Ok();
    }
    
    [Route("api/change/username")]
    [HttpPut]
    [Authorize(Policy = nameof(Policy.UserPolicy))]
    public async Task<IActionResult> ChangeUsername([FromForm] ChangeUsername request, CancellationToken cancellationToken)
    {
        string authHeader = Request.Headers["Authorization"];
        if (authHeader == null || !authHeader.StartsWith("Bearer "))
        {
            return BadRequest("Invalid token.");
        }
        var jwt = authHeader.Substring("Bearer ".Length).Trim();
        Console.WriteLine(jwt);
        var userId = authService.GetUserId(jwt);
        if (userId.IsFailure)
        {
            return BadRequest(userId.Error);
        }
        var response = await authService.ChangeUsername(userId.Value, request.NewUsername);
        if (response.IsFailure)
        {
            return BadRequest(response.Error);
        }
        
        return Ok();
    }
}