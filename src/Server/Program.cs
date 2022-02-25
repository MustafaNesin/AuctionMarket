using AuctionMarket.Server.Application.Abstractions;
using AuctionMarket.Server.Application.Commands;
using AuctionMarket.Server.Application.Hubs;
using AuctionMarket.Server.Application.Profiles;
using AuctionMarket.Server.Application.Validators;
using AuctionMarket.Server.Domain.Entities;
using AuctionMarket.Server.Infrastructure;
using AuctionMarket.Server.Infrastructure.Services;
using AuctionMarket.Server.Persistence.Contexts;
using AuctionMarket.Server.Persistence.Seeds;
using AuctionMarket.Shared.Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<IAppDbContext, AppDbContext>(
    options => options.UseSqlite(builder.Configuration.GetConnectionString("App"),
        sqliteOptions => sqliteOptions.MigrationsAssembly(
            typeof(AppDbContext).Assembly.GetName().Name)));

if (builder.Environment.IsDevelopment())
    builder.Services.AddScoped<IDataSeeder, TestDataSeeder>();

builder.Services.AddSingleton<ICurrentAccountService, CurrentAccountService>();
builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddMediatR(typeof(LoginAccountCommandHandler));

builder.Services.AddProblemDetails(options =>
{
    options.Map<ValidationException>((ctx, ex) =>
    {
        var factory = ctx.RequestServices.GetRequiredService<ProblemDetailsFactory>();

        var errors = ex.Errors
            .GroupBy(failure => failure.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(failure => failure.ErrorMessage).ToArray());

        return factory.CreateValidationProblemDetails(ctx, errors);
    });

    options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
    options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);
    options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
});

builder.Services.AddValidatorsFromAssemblyContaining<LoginAccountCommandValidatorBase>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginAccountCommandValidator>();
builder.Services.AddSingleton<ITicketStore, MemoryCacheTicketStore>();
builder.Services.AddScoped<CustomCookieAuthenticationEvents>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddOptions<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme)
    .Configure(options => options.EventsType = typeof(CustomCookieAuthenticationEvents))
    .Configure<ITicketStore>((o, ticketStore) => o.SessionStore = ticketStore);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddSignalR();
builder.Services.AddRazorPages();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
});

builder.Services.AddControllersWithViews()
    .AddFluentValidation()
    .AddProblemDetailsConventions();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseWebAssemblyDebugging();
else
{
    app.UseResponseCompression();
    app.UseHsts();
}

{
    var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
    await using var scope = scopeFactory.CreateAsyncScope();
    var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();

    foreach (var seeder in seeders)
        await seeder.EnsureSeedsAsync();
}

app.UseProblemDetails();
app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapHub<AppHub>("/Hub");
app.MapFallbackToFile("index.html");

app.Run();