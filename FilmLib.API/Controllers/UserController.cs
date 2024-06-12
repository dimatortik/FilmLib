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
        
        return Ok(response.Value);
    }
    
    [Route("api/user/logout")]
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> LogoutUser()
    {
        HttpContext.Response.Cookies.Delete("cosy");
        return Ok();
    }
    
    [Route("api/change/password")]
    [HttpPut]
    [Authorize(Policy = nameof(Policy.AdminPolicy))]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePassword request, CancellationToken cancellationToken)
    {
        var jwt = HttpContext.Request.Cookies["cosy"];
        var userId = authService.GetUserId(jwt);
        if (userId.IsFailure)
        {
            return BadRequest(userId.Error);
        }
        var response = await authService.ChangePassword(userId.Value, request.Password, request.NewPassword);
        if (response.IsFailure)
        {
            return BadRequest(response.Error);
        }
        
        return Ok();
    }
    
    [Route("api/change/username")]
    [HttpPut]
    [Authorize(Policy = nameof(Policy.AdminPolicy))]
    public async Task<IActionResult> ChangeUsername([FromBody] ChangeUsername request, CancellationToken cancellationToken)
    {
        var jwt = HttpContext.Request.Cookies["cosy"];
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