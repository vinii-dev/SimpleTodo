using Microsoft.OpenApi.Models;
using SimpleTodo.Api.Extensions;
using SimpleTodo.Application;
using SimpleTodo.Domain.Options;
using SimpleTodo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerDocumentation();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer(builder.Configuration);

builder.Services.Configure<TokenOptions>(
    builder.Configuration.GetSection(TokenOptions.PATH));


//
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
