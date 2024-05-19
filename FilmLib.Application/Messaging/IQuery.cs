using CSharpFunctionalExtensions;
using MediatR;

namespace FilmLib.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
