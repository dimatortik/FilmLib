
using FilmLib.API.Contracts;
using FilmLib.API.Contracts.Comment;
using FilmLib.Application.Auth;
using FilmLib.Application.FilmComments.Commands.Create;
using FilmLib.Application.FilmComments.Commands.Delete;
using FilmLib.Application.FilmComments.Queries.GetAllByFilmId;
using FilmLib.Infrastructure.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmLib.API.Controllers;
[ApiController]
public class FilmCommentController(IMediator sender, AuthService auth) : ControllerBase
{
    [Route("/api/film/{id}/comment")]
    [HttpPost]
    [Authorize(Policy = nameof(Policy.UserPolicy))]
    public async Task<IActionResult> AddComment(
        [FromBody] CommentRequest request,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        string authHeader = Request.Headers["Authorization"];
        if (authHeader == null || !authHeader.StartsWith("Bearer "))
        {
            return BadRequest("Invalid token.");
        }
        var jwt = authHeader.Substring("Bearer ".Length).Trim();
        var userId = auth.GetUserId(jwt);
        if (userId.IsFailure)
        {
            return BadRequest(userId.Error);
        }
        var command = new CreateFilmCommentCommand(
            request.Comment,
            userId.Value,
            id);
        var result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
    
    [Route("/api/film/{id}/comment/{commentId}")]
    [HttpDelete]
    [Authorize(Policy = nameof(Policy.AdminPolicy))]
    public async Task<IActionResult> DeleteComment(
        [FromRoute] Guid id,
        [FromRoute] Guid commentId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteFilmCommentCommand(
            commentId);
        var result = await sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
    
    [Route("/api/film/{id}/comments")]
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetComments(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetAllCommentsByFilmIdQuery(id);
        var result = await sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
    
}