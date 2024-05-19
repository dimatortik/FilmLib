using CSharpFunctionalExtensions;
using FilmLib.API.Contracts;
using FilmLib.Application.FilmActions.Commands.CreateFilm;
using FilmLib.Application.FilmActions.Queries.GetFilms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FilmLib.API.Controllers;

[ApiController]
public class HomeController : ControllerBase
{
    private readonly ISender _sender;

    public HomeController(ISender sender)
    {
        _sender = sender;
    }
    [Route("api/film")]
    [HttpPost]
    public async Task<IActionResult> AddFilm([FromBody] FilmDto request, CancellationToken cancellationToken)
    {
        // var request = new FilmDto(
        //     "https://www.google.com", 
        //     "Title", 
        //     " ", 
        //     2021, 
        //     "Country", 
        //     "Director", 
        //     "https://www.google.com");
        
        var command = new CreateFilmCommand(request.TitleImageLink, request.Title, request.Description,
            request.Year, request.Country, request.Director, request.FilmVideoLink);
        var result = await _sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
    
    [Route("api/film")]
    [HttpGet]
    public async Task<IActionResult> GetFilms(CancellationToken cancellationToken)
    {
        var films = await _sender.Send(new GetFilmsQuery(), cancellationToken);
        
        return Ok(films.Value);
    }
}