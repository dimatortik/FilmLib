using FilmLib.Application.Messaging;

namespace FilmLib.Application.FilmComments.Commands.Create;

public record CreateFilmCommentCommand(
    string Body,
    Guid UserId,
    Guid FilmId) : ICommand;
