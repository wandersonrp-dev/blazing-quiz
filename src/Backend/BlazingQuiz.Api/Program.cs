using BlazingQuiz.Api;
using BlazingQuiz.Api.Data;
using BlazingQuiz.Api.Endpoints;
using BlazingQuiz.Api.Entities;
using BlazingQuiz.Api.Extensions;
using BlazingQuiz.Api.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var MyWebAppCors = "MyWebAppCors";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration.GetJwtIssuer(),
        ValidateAudience = true,
        ValidAudience = builder.Configuration.GetJwtAudience(),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetJwtSecret())),
        ValidateIssuerSigningKey = true,
    };
});

builder.Services.AddAuthorization();

builder.Services.AddDi();

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

builder.Services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddDbContext<QuizDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Quiz")));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyWebAppCors, policy =>
    {
        var allowedOriginsString = builder.Configuration.GetRequiredSection("AppSettings:Cors:AllowedOrigins").Value ?? 
            throw new ArgumentNullException("Provide allowed origins");

        var allowedOrigins = allowedOriginsString.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

#if DEBUG
ApplyMigrations(app.Services);
#endif

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyWebAppCors);

app.UseAuthentication();

app.UseAuthorization();

app.MapAuthEndpoints();

app.Run();

static void ApplyMigrations(IServiceProvider serviceProvider)
{
    var scope = serviceProvider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<QuizDbContext>();    

    if(context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}
