using ASP111.Data;
using ASP111.Services;
using ASP111.Middleware;
using ASP111.Services.Hash;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using ASP111.Services.Email;
using ASP111.Services.AuthUser;
using ASP111.Services.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddSingleton<DateService>();
builder.Services.AddSingleton<IDateService, DateService>();

builder.Services.AddScoped<TimeService>();
builder.Services.AddTransient<DateTimeService>();

builder.Services.AddSingleton<ValidatorService>();

builder.Services.AddSingleton<IHashService ,Md5HashService>();

builder.Services.AddSingleton<IEmailService,GmailService>();

builder.Services.AddSingleton<IValidationService, ValidationServiceV1>();

builder.Services.AddScoped<IAuthUserService, ClaimsAuthUserService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(15 * 60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


String? connectionString = builder.Configuration.GetConnectionString("PlanetScale");
MySqlConnection connection = new MySqlConnection(connectionString);

builder.Services.AddDbContext<DataContext>(options =>options.UseMySql(connection,
    ServerVersion.AutoDetect(connection), 
    serverOptions =>serverOptions
            .MigrationsHistoryTable(tableName: HistoryRepository.DefaultTableName,schema: "asp111")
            .SchemaBehavior(MySqlSchemaBehavior.Translate,
            (schema, table) => $"{schema}_{table}")
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseMarker();

app.UseAuthorization();

app.UseSession();

app.UseAuthSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();