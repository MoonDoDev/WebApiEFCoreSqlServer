using Application;
using Persistence;
using WebApi;

var builder = WebApplication.CreateBuilder( args );

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddWebApiServices( builder.Configuration );

builder.Services.AddExceptionHandler<GlobalExceptionsHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddPersistence( builder.Configuration );

var app = builder.Build();

// Configure the HTTP request pipeline.
if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapWebApiServices();

app.MapControllers();
app.UseExceptionHandler();

app.Run();
