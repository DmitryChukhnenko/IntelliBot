using IntelliBot.Core.Config;
using IntelliBot.Core.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

string[]? allowedOrigins = builder.Configuration["ALLOWED_ORIGINS"]?.Split(',') 
    ?? ["http://localhost:5051", "http://frontend:3000"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("NextFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddSingleton<IAssistantService, OpenrouterAssistantService>();
builder.Services.AddHostedService<BackgroundCleanupService>();

builder.Services.AddControllers();

if (builder.Environment.IsDevelopment())
{    
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

builder.Services.AddHealthChecks();

var memoryExpiration = builder.Configuration.GetValue<int>("MEMORY_EXPIRATION_MINUTES", 30);
var maxHistory = builder.Configuration.GetValue<int>("MAX_CONVERSATION_HISTORY", 10);
builder.Services.AddSingleton(new MemoryConfig(memoryExpiration, maxHistory));

WebApplication app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Chat API"));
}

app.UseCors("NextFrontend");

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();