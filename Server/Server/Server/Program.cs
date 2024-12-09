using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Server.Model.Data;
using Server.Model.Images;
using Server.Model.Managers;
using Serilog; // Assurez-vous d'importer Serilog

var builder = WebApplication.CreateBuilder(args);

// Vide le contenu des anciens logs 
var logFilePath = "Logs/logs.txt";
if (File.Exists(logFilePath))
{
    File.Delete(logFilePath); // Supprime le fichier existant
}

// Configurer Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("Logs/logs.txt", fileSizeLimitBytes: 10_000_000, retainedFileCountLimit: 5) // Journalisation dans un fichier
    .CreateLogger();

// Supprimez les fournisseurs de journalisation par défaut
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(); // Ajoutez Serilog comme fournisseur de journalisation

// Ajoutez des services au conteneur
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<LocalImageSettings>(builder.Configuration.GetSection("LocalImageSettings"));

// Injection de dépendances
builder.Services.AddScoped<IDatabase, SQLiteDatabase>(provider =>
{
    var config = provider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
    return new SQLiteDatabase(config.DefaultConnection);
});

builder.Services.AddScoped<IFileUploader, LocalFileUploader>(provider =>
{
    var imageSettings = provider.GetRequiredService<IOptions<LocalImageSettings>>().Value;
    var logger = provider.GetRequiredService<ILogger<LocalFileUploader>>();
    return new LocalFileUploader(imageSettings.ImageDirectory, logger);
});

builder.Services.AddScoped<TokenManager>(provider =>
{
    var tokenDao = provider.GetRequiredService<ITokenDAO>();
    var logger = provider.GetRequiredService<ILogger<TokenManager>>();
    return new TokenManager(tokenDao, logger);
});

builder.Services.AddScoped<ImageManager>();

builder.Services.AddScoped<UserManager>(provider =>
{
    var userDao = provider.GetRequiredService<IUserDAO>();
    var imageManager = provider.GetRequiredService<ImageManager>();
    var tokenManager = provider.GetRequiredService<TokenManager>();
    var logger = provider.GetRequiredService<ILogger<UserManager>>();
    return new UserManager(userDao, imageManager, tokenManager, logger);
});

builder.Services.AddScoped<GameManager>();



builder.Services.AddScoped<IUserDAO, UserDAO>(provider =>
{
    var database = provider.GetRequiredService<IDatabase>();
    var logger = provider.GetRequiredService<ILogger<UserDAO>>();
    return new UserDAO(database, logger);
});

builder.Services.AddScoped<ITokenDAO, TokenDAO>(provider =>
{
    var database = provider.GetRequiredService<IDatabase>();
    var logger = provider.GetRequiredService<ILogger<TokenDAO>>();
    return new TokenDAO(database, logger);
});

builder.Services.AddScoped<IGameDAO, GameDAO>(provider =>
{
    var database = provider.GetRequiredService<IDatabase>();
    var logger = provider.GetRequiredService<ILogger<GameDAO>>();
    return new GameDAO(database, logger);
});

builder.Services.AddScoped<IMessageDAO, MessageDAO>(provider =>
{
    var database = provider.GetRequiredService<IDatabase>();
    return new MessageDAO(database);
});

builder.Services.AddScoped<MessageManager>(provider =>
{
    var messageDao = provider.GetRequiredService<IMessageDAO>();
    var tokenDAO = provider.GetRequiredService<ITokenDAO>();
    var logger = provider.GetRequiredService<ILogger<MessageManager>>();
    return new MessageManager(tokenDAO, messageDao, logger);
});

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
