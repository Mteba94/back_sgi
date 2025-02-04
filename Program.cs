using System.Security.Claims;
using WebApi_SGI_T.Imp;
using WebApi_SGI_T.Imp.Authentication;
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

builder.Services.AddAuthorization(options =>
{
    foreach (Permission permission in Enum.GetValues(typeof(Permission)))
    {
        options.AddPolicy(permission.ToString(), policy => policy.RequireAssertion(context =>
        {
            var httpContextAccessor = context.Resource as HttpContext;
            var userIdClaim = httpContextAccessor?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return false;
            }

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return false;
            }

            var permissionService = httpContextAccessor?.RequestServices.GetRequiredService<PermissionService>();
            var permissions = permissionService?.GetPermissionsByUserIdAsync(userId).Result;
            return permissions.Contains(permission);
        }));
    }
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddHttpContextAccessor();

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


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.Use(async (context, next) =>
//{
//    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; img-src 'self' data:; media-src mediastream:; script-src 'self'; style-src 'self'; frame-ancestors 'none'; form-action 'self'; ");
//    await next();
//});


app.UseAuthentication();

app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<DataInitial>();
    await initializer.SeedRolesAsync();
    await initializer.UpdateExistingRolesPermissionsAsync(); // Ejecuta la actualización

    app.MapControllers();
}

app.Run();

public partial class Program { }