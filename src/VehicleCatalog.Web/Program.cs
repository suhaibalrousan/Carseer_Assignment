using VehicleCatalog.Application.Services;
using VehicleCatalog.Application.Mappings;
using VehicleCatalog.Domain.Interfaces;
using VehicleCatalog.Infrastructure.ExternalServices;
using VehicleCatalog.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


builder.Services.AddMemoryCache();

builder.Services.AddAutoMapper(typeof(VehicleMappingProfile));

builder.Services.AddHttpClient<NHTSAApiClient>(client =>
{
    var baseUrl = builder.Configuration["NHTSAApi:BaseUrl"] 
        ?? throw new InvalidOperationException("NHTSAApi:BaseUrl is not configured in appsettings.json");
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddScoped<VehicleService>();

builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();

builder.Services.AddHealthChecks();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapHealthChecks("/health");
app.MapGet("/health", () => Results.Ok("Healthy"));

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
