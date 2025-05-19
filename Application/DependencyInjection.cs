using Application.Abstractions;
using Application.Employees;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication( this IServiceCollection services )
	{
		services.AddScoped<CreateCommandHandler>();
		services.AddScoped<UpdateCommandHandler>();
		services.AddScoped<DeleteCommandHandler>();
		services.AddScoped<GetByIdQueryHandler>();
		services.AddScoped<GetAllQueryHandler>();

		return services;
	}
}