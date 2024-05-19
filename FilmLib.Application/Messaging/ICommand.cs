using MediatR;
using CSharpFunctionalExtensions;

namespace FilmLib.Application.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}

