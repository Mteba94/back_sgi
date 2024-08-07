using WebApi_SGI_T.Imp.Extensions;
using WebApi_SGI_T.Imp.FileStorage;
using WebApi_SGI_T.Models.Extensions;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

var Cors = "Cors";

builder.Services.AddInjection(Configuration);
builder.Services.AddInjectionImp(Configuration);
builder.Services.AddAuthentication(Configuration);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: Cors,
               builder =>
               {
                   builder.WithOrigins("*");
                   builder.AllowAnyHeader();
                   builder.AllowAnyMethod();

               });
});

var app = builder.Build();

app.UseCors(Cors);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }