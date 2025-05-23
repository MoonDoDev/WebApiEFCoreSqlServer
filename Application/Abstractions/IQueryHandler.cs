namespace Application.Abstractions;

public interface IQueryHandler<TQuery, TResult>
{
	Task<TResult> Execute( TQuery query, CancellationToken cancellationToken = default );
}

public interface IQueryHandler<TResult>
{
	Task<TResult> Execute( CancellationToken cancellationToken = default );
}
