using Core.Managers.Users;
using Core.Profiles.Users;
using Core.Repositories.Cars;
using Core.Repositories.Users;
using Core.Services;
using Core.Utilities;
using Core.Profiles.Subscriptions;
using FastEndpoints;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Core.Services;
using Core.Repositories.Subscriptions;
using Core.BackgroundServices.Notifications;
using Core.Queues;
using Core.Middleware.Security;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("CarShopDb");
builder.Services.AddDbContext<CarShopDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ICarRepository, CarRepository>();
builder.Services.AddTransient<ISecurityRepository, SecurityRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserManager, UserManager>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<JwtTokenGenerator>();
builder.Services.AddHostedService<NotificationService>();
builder.Services.AddSingleton<INewCarsForSaleQueue, NewCarsForSaleQueue>();

builder.Services.AddTransient<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddFastEndpoints();
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(Program).Assembly));

//Mappers
builder.Services.AddAutoMapper(typeof(CarProfile));
builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddAutoMapper(typeof(SubscriptionProfile));

// Enable sessions
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("https://localhost:44420")
            .AllowAnyHeader()
            .WithMethods("GET", "POST", "PUT");
        });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<CarShopDbContext>();
    dbContext.Database.Migrate();
    await InitialSetup.InsertInitialDbData(dbContext);
}


app.UseSession();
app.UseRouting();
app.UseCors();
app.UseMiddleware<RestrictedIpMiddleware>();

app.UseFastEndpoints();

app.Run();