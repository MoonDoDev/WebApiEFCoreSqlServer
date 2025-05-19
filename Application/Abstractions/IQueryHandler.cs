namespace Application.Abstractions;

public interface IQueryHandler<TQuery, TResult>
{
	Task<TResult> Handle( TQuery query, CancellationToken cancellationToken = default );
}

public interface IQueryHandler<TResult>
{
	Task<TResult> Handle( CancellationToken cancellationToken = default );
}
