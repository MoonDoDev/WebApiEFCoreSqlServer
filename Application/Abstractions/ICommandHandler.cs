namespace Application.Abstractions;

public interface ICommandHandler<TCommand>
{
	Task Handle( TCommand command, CancellationToken cancellationToken = default );
}

public interface ICommandHandler<TCommand, TResult>
{
	Task<TResult> Handle( TCommand command, CancellationToken cancellationToken = default );
}