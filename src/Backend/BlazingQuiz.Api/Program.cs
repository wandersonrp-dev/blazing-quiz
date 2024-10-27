using BlazingQuiz.Api;
using BlazingQuiz.Api.Data;
using BlazingQuiz.Api.Endpoints;
using BlazingQuiz.Api.Entities;
using BlazingQuiz.Api.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDi();

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

builder.Services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddDbContext<QuizDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Quiz")));

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
