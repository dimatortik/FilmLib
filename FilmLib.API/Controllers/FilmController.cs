using FilmLib.API.Contracts.Film;
using FilmLib.Application.Auth;
using FilmLib.Application.Films.Commands.AddVote;
using FilmLib.Application.Films.Commands.Create;
using FilmLib.Application.Films.Commands.Delete;
using FilmLib.Application.Films.Commands.Update;
using FilmLib.Application.Films.Queries.GetAll;
using FilmLib.Application.Films.Queries.GetById;
using FilmLib.Application.Films.Queries.GetFilmsByActorId;
using FilmLib.Application.Films.Queries.GetFilmsByGenreId;
using FilmLib.Infrastructure.Auth;
using FilmLib.Infrastructure.CloudStorage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmLib.API.Controllers;

[ApiController]
public class FilmController(IMediator sender, ICloudStorageService storageService, AuthService authService) : ControllerBase
{
    [Route("api/film")]
    [HttpPost]
    [Authorize(Policy = nameof(Policy.AdminPolicy))]
    public async Task<IActionResult> AddFilm(
        [FromForm] FilmRequest request,
        CancellationToken cancellationToken)
    {
        var titleImageLink = await storageService.UploadFileAsync(request.TitleImage, request.Title);
        if (titleImageLink.IsFailure)
            return BadRequest(titleImageLink.Error);

        var filmVideoLink = await storageService.UploadFileAsync(request.FilmVideo, request.Title);
        if (filmVideoLink.IsFailure)
            return BadRequest(filmVideoLink.Error);

        var command = new CreateFilmCommand(
            titleImageLink.Value,
            request.Title,
            request.Description,
            request.Year,
            request.Country,
            request.Director,
            filmVideoLink.Value,
            request.Actors,
            request.Genres);

        var result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [Route("api/film/{id}")]
    [HttpPut]
    [Authorize(Policy = nameof(Policy.AdminPolicy))]
    public async Task<IActionResult> UpdateFilm(
        [FromForm] FilmRequest request,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        // var titleImageLink = "";
        // var filmVideoLink = "";
        
            var titleImageLinkResult = await storageService.UploadFileAsync(request.TitleImage, request.Title);
            if (titleImageLinkResult.IsFailure)
                return BadRequest(titleImageLinkResult.Error);

            var filmVideoLinkResult = await storageService.UploadFileAsync(request.FilmVideo, request.Title);
            if (filmVideoLinkResult.IsFailure)
                return BadRequest(filmVideoLinkResult.Error);
            

        var command = new EditFilmCommand(
            id,
            titleImageLinkResult.Value,
            request.Title,
            request.Description,
            request.Year,
            request.Country,
            request.Director,
            filmVideoLinkResult.Value);

        var result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [Route("api/film/{id}")]
    [HttpDelete]
    [Authorize(Policy = nameof(Policy.AdminPolicy))]
    public async Task<IActionResult> DeleteFilm(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteFilmCommand(id);
        var result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [Route("api/films")]
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetFilms(
        string? searchTerm,
        string? sortColumn,
        string? sortOrder,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var result = await sender
            .Send(new GetFilmsQuery(
                searchTerm,
                sortColumn,
                sortOrder,
                page,
                pageSize), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [Route("api/film/{id}")]
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetFilm(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender
            .Send(new GetFilmByIdQuery(
                id), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [Route("api/film/{id}/vote")]
    [HttpGet]
    [Authorize(Policy = nameof(Policy.UserPolicy))]
    public async Task<IActionResult> VoteFilm(
        [FromRoute] Guid id,
        [FromBody] int voteValue,
        CancellationToken cancellationToken)
    {
        var votedFilms = HttpContext.Session.GetString("votedFilms");
        if (votedFilms != null && votedFilms.Split(',').Contains(id.ToString()))
        {
            return BadRequest("You have already voted for this film.");
        }

        var command = new AddVoteCommand(id, voteValue);
        var result = await sender.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            HttpContext.Session.SetString("votedFilms", votedFilms != null ? $"{votedFilms},{id}" : id.ToString());
        }

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [Route("api/actor/{id}/films")]
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetFilmsByActorId(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetFilmsByActorIdQuery(id), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
    
    [Route("api/genre/{id}/films")]
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetFilmsByGenreId(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetFilmsByGenreIdQuery(id), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}    

