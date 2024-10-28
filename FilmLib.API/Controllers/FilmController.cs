using FilmLib.API.Contracts.Film;
using FilmLib.Application.Films.Commands.AddRate;
using FilmLib.Application.Films.Commands.Create;
using FilmLib.Application.Films.Commands.Delete;
using FilmLib.Application.Films.Commands.Update;
using FilmLib.Application.Films.Queries.GetAll;
using FilmLib.Application.Films.Queries.GetById;
using FilmLib.Application.Films.Queries.GetFilmsByActorId;
using FilmLib.Application.Films.Queries.GetFilmsByGenreId;
using FilmLib.Application.Interfaces;
using FilmLib.Infrastructure.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmLib.API.Controllers;

[ApiController]
public class FilmController(IMediator sender, ICloudStorageService storageService) : ControllerBase
{
    [Route("api/film")]
    [HttpPost]
    [Authorize(Policy = nameof(Policy.AdminPolicy))]
    public async Task<IActionResult> AddFilm(
        [FromForm] FilmCreateRequest createRequest,
        CancellationToken cancellationToken)
    {
        var titleImageLink = await storageService.UploadFileAsync(createRequest.TitleImage, createRequest.Title);
        if (titleImageLink.IsFailure)
            return BadRequest(titleImageLink.Error);

        var filmVideoLink = await storageService.UploadFileAsync(createRequest.FilmVideo, createRequest.Title);
        if (filmVideoLink.IsFailure)
            return BadRequest(filmVideoLink.Error);

        var command = new CreateFilmCommand(
            titleImageLink.Value,
            createRequest.Title,
            createRequest.Description,
            createRequest.Year,
            createRequest.Country,
            createRequest.Director,
            filmVideoLink.Value,
            createRequest.Actors,
            createRequest.Genres);

        var result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }

    [Route("api/film/{id}")]
    [HttpPut]
    [Authorize(Policy = nameof(Policy.AdminPolicy))]
    public async Task<IActionResult> UpdateFilm(
        [FromForm] FilmCreateRequest createRequest,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
            var titleImageLinkResult = await storageService.UploadFileAsync(createRequest.TitleImage, createRequest.Title);
            if (titleImageLinkResult.IsFailure)
                return BadRequest(titleImageLinkResult.Error);

            var filmVideoLinkResult = await storageService.UploadFileAsync(createRequest.FilmVideo, createRequest.Title);
            if (filmVideoLinkResult.IsFailure)
                return BadRequest(filmVideoLinkResult.Error);
            

            var command = new EditFilmCommand(
                id,
                titleImageLinkResult.Value,
                createRequest.Title,
                createRequest.Description,
                createRequest.Year,
                createRequest.Country,
                createRequest.Director,
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
        [FromQuery]List<string>? genres,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var result = await sender
            .Send(new GetFilmsQuery(
                searchTerm,
                sortColumn,
                sortOrder,
                genres,
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
    [HttpPost]
    [Authorize(Policy = nameof(Policy.UserPolicy))]
    public async Task<IActionResult> RateFilm(
        [FromRoute] Guid id,
        [FromForm] RateFilmRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddRateCommand(id, request.RatingValue, request.UserId);
        var result = await sender.Send(command, cancellationToken);

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

