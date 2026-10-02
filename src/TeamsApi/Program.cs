using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TeamsApi.Data;
using TeamsApi.Mapping;
using TeamsApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure EF Core with SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=teams.db";
    options.UseSqlite(connectionString);
});

// Configure AutoMapper
var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperProfile());
});
builder.Services.AddSingleton(mapperConfig.CreateMapper());

// Register Repository in DI Container
builder.Services.AddScoped<ITeamRepository, TeamRepository>();

var app = builder.Build();

// Ensure Database is created and seeded (Code First)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
