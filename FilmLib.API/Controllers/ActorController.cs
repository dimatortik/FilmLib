using FilmLib.API.Contracts.Actor;
using FilmLib.Application.Actors.Commands.Create;
using FilmLib.Application.Actors.Commands.Delete;
using FilmLib.Application.Actors.Commands.Update;
using FilmLib.Application.Actors.Queries.GetActorsByFilmId;
using FilmLib.Application.Actors.Queries.GetAll;
using FilmLib.Application.Actors.Queries.GetById;
using FilmLib.Application.Auth;
using FilmLib.Infrastructure.Auth;
using FilmLib.Infrastructure.CloudStorage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmLib.API.Controllers;

public class ActorController(IMediator sender, ICloudStorageService cloud, AuthService authService) : ControllerBase
{
    [Route("api/actor")]
    [HttpPost]
    [Authorize(Policy = nameof(Policy.AdminPolicy))]
    public async Task<IActionResult> AddActor(
        [FromForm] ActorRequest request,
        CancellationToken cancellationToken)
    {
        var actorImageLink = await cloud.UploadFileAsync(request.ActorImage, request.ActorName);
        if (actorImageLink.IsFailure)
        {
            return BadRequest(actorImageLink.Error);
        }
        var command = new CreateActorCommand(
            actorImageLink.Value,
            request.ActorName,
            request.ActorDescription);
        var result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
    
    
    [Route("api/actor/{id}")]
    [HttpDelete]
    [Authorize(Policy = nameof(Policy.AdminPolicy))]
    public async Task<IActionResult> DeleteActor(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteActorCommand(id);
        var result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
    
    [Route("api/actor/{id}")]
    [HttpPut]
    [Authorize(Policy = nameof(Policy.AdminPolicy))]
    public async Task<IActionResult> UpdateActor(
        [FromForm] ActorRequest? request,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var actorImageLink = await cloud.UploadFileAsync(request.ActorImage, request.ActorName);
        var command = new UpdateActorCommand(
            id,
            actorImageLink.Value,
            request.ActorName,
            request.ActorDescription);
        var result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
    
    [Route("api/film/{id}/actors")]
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetActorsByFilmId(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetActorsByFilmIdQuery(id);
        var result = await sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }
    
    [Route("api/actors")]
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetActors(
        string searchTerm,
        CancellationToken cancellationToken)
    {
        var query = new GetActorsQuery(searchTerm);
        var result = await sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }
    
    [Route("api/actor/{id}")]
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetActorById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetActorByIdQuery(id);
        var result = await sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }
}