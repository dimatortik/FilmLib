using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FilmLib.API.Controllers;

[ApiController]
public class HomeController : ControllerBase
{
    private readonly ISender _sender;

    // public HomeController(ISender sender)
    // {
    //     _sender = sender;
    // }
    // [Route("api/film")]
    // [HttpPost]
    // public async Task<IActionResult> AddFilm([FromBody] FilmRequest request, CancellationToken cancellationToken)
    // {
    //     var command = new CreateFilmCommand(
    //         request.TitleImageLink,
    //         request.Title,
    //         request.Description,
    //         request.Year,
    //         request.Country,
    //         request.Director,
    //         request.FilmVideoLink,
    //         request.Actors,
    //         request.Genres);
    //     
    //     var result = await _sender.Send(command, cancellationToken);
    //     return result.IsSuccess ? Ok() : BadRequest(result.Error);
    // }
    //
    // [Route("api/films")]
    // [HttpGet]
    // [Authorize(Policy = nameof(Policy.AdminPolicy))]
    // public async Task<IActionResult> GetFilms(
    //     string? searchTerm, 
    //     string? sortColumn, 
    //     string? sortOrder,
    //     int page,
    //     int pageSize,
    //     CancellationToken cancellationToken)
    // {
    //     var result = await _sender
    //         .Send(new GetFilmsQuery(
    //             searchTerm, 
    //             sortColumn, 
    //             sortOrder, 
    //             page, 
    //             pageSize), cancellationToken);
    //     
    //     if (!result.IsSuccess)
    //     {
    //         return BadRequest(result.Error);
    //     }
    //
    //     var films = result.Value;
    //     return Ok(films);
    // }
    
}

