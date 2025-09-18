using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DbContext>(options =>
{
    options.UseInMemoryDatabase("db");
    options.UseOpenIddict();
});

builder
    .Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore().UseDbContext<DbContext>();
    })
    .AddClient(options =>
    {
        options.AllowAuthorizationCodeFlow().AllowRefreshTokenFlow();

        options.AddDevelopmentEncryptionCertificate().AddDevelopmentSigningCertificate();

        options.UseAspNetCore().EnableRedirectionEndpointPassthrough();

        options.UseSystemNetHttp();

        var spotifySettings = builder.Configuration.GetSection("SpotifySettings");
        options
            .UseWebProviders()
            .AddSpotify(spotifyOptions =>
            {
                spotifyOptions
                    .SetClientId(spotifySettings.GetValue<string>("ClientId")!)
                    .SetClientSecret(spotifySettings.GetValue<string>("ClientSecret")!)
                    .SetRedirectUri("auth/callback/spotify");
            });
    });

builder
    .Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "__Host_app";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        // options.LoginPath = "/challenge";
        // options.AccessDeniedPath = "/auth/forbidden";
    });
builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    // app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
