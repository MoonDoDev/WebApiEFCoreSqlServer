using Application.Employees;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication( this IServiceCollection services )
	{
		services.AddScoped<ICreateCommandHandler, CreateCommandHandler>();
		services.AddScoped<IUpdateCommandHandler, UpdateCommandHandler>();
		services.AddScoped<IDeleteCommandHandler, DeleteCommandHandler>();
		services.AddScoped<IQueryOneHandler, GetByIdQueryHandler>();
		services.AddScoped<IQueryAllHandler, GetAllQueryHandler>();

		return services;
	}
}