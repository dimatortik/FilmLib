using CSharpFunctionalExtensions;
using FilmLib.API.Contracts;
using FilmLib.API.Contracts.Genre;
using FilmLib.Application.Genres.Commands.Create;
using FilmLib.Application.Genres.Commands.Delete;
using FilmLib.Application.Genres.Commands.Update;
using FilmLib.Application.Genres.Queries.GetAll;
using FilmLib.Application.Genres.Queries.GetById;
using FilmLib.Application.Genres.Queries.GetGenresByFilmId;
using FilmLib.Infrastructure.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmLib.API.Controllers;
[ApiController]
public class GenreController(IMediator sender) : ControllerBase
{
    [Route("api/genre")]
    [HttpPost]
    [Authorize(Policy = nameof(Policy.AdminPolicy))]
    public async Task<IActionResult> AddGenre(
        [FromBody] GenreRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddGenreCommand(
            request.Title,
            request.Description);
        var result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
    
    [Route("api/genre/{id}")]
    [HttpDelete]
    [Authorize(Policy = nameof(Policy.AdminPolicy))]
    public async Task<IActionResult> DeleteGenre(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteGenreCommand(
            id);
        var result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
    
    
    [Route("api/genre/{id}")]
    [HttpPut]
    [Authorize(Policy = nameof(Policy.AdminPolicy))]
    public async Task<IActionResult> UpdateGenre(
        [FromRoute] int id,
        [FromBody] GenreRequest request,
        CancellationToken cancellationToken)
    {
        var command = new EditGenreCommand(
            id,
            request.Title,
            request.Description);
        var result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
    
    [Route("api/genres")]
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetGenres(
        [FromQuery] string? searchTerm,
        CancellationToken cancellationToken)
    {
        var query = new GetAllGenresQuery(searchTerm);
        var result = await sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
    
    [Route("api/genre/{id}")]
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetGenre(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var query = new GetGenreQuery(id);
        var result = await sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
    
    [Route("api/film/{id}/genres")]
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetGenresByFilm(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetGenresByFilmIdQuery(id);
        var result = await sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}