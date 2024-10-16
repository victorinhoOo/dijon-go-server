using Microsoft.Extensions.Options;
using Server.Model.Data;
using Server.Model.Images;
using Server.Model.Managers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<LocalImageSettings>(builder.Configuration.GetSection("LocalImageSettings"));

// Injection de dépendances
builder.Services.AddScoped<IDatabase, MySqlDatabase>(provider =>
{
    var config = provider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
    return new MySqlDatabase(config.DefaultConnection);
});
builder.Services.AddScoped<IFileUploader, LocalFileUploader>(provider =>
{
    var imageSettings = provider.GetRequiredService<IOptions<LocalImageSettings>>().Value;
    return new LocalFileUploader(imageSettings.ImageDirectory);
});
builder.Services.AddScoped<TokenManager>();
builder.Services.AddScoped<UserManager>();
builder.Services.AddScoped<ImageManager>();
builder.Services.AddScoped<GameManager>();
builder.Services.AddScoped<IUserDAO, UserDAO>();
builder.Services.AddScoped<ITokenDAO, TokenDAO>();
builder.Services.AddScoped<IGameDAO, GameDAO>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    .SetIsOriginAllowed(origin => true));

app.UseAuthorization();

app.MapControllers();

app.Run();
