namespace Sgd.Application.Common.Messaging;

public interface IQuery<TResponse> : IRequest<ErrorOr<TResponse>> { }
