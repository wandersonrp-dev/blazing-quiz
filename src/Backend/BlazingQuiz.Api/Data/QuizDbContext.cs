using BlazingQuiz.Api.Entities;
using BlazingQuiz.Api.Enums.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlazingQuiz.Api.Data;

public class QuizDbContext : DbContext
{
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly string _adminName;
    private readonly string _adminEmail;
    private readonly string _adminPassword;

    public QuizDbContext(DbContextOptions<QuizDbContext> options, IPasswordHasher<User> passwordHasher, IConfiguration configuration) : base(options)
    {
        _passwordHasher = passwordHasher;
        _adminName = configuration.GetRequiredSection("AppSettings:Admin:Name").Value ?? string.Empty;
        _adminEmail = configuration.GetRequiredSection("AppSettings:Admin:Email").Value ?? string.Empty;
        _adminPassword = configuration.GetRequiredSection("AppSettings:Admin:Password").Value ?? string.Empty;
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Option> Options { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<StudentQuiz> StudentQuizzes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var admin = new User
        {
            Id = 1,
            Name = _adminName,
            Email = _adminEmail,
            Role = Role.Admin,    
            IsApproved = true,
        };

        admin.Password = _passwordHasher.HashPassword(admin, _adminPassword);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasData(admin);

            entity.Property(e => e.Role)
                .HasConversion<string>();            
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}
