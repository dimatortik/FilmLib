using FilmLib.Application.Messaging;

namespace FilmLib.Application.FilmComments.Commands.Delete;

public record DeleteFilmCommentCommand(
    Guid Id) : ICommand;
    
