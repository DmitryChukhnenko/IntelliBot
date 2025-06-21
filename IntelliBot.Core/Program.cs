using IntelliBot.Core.Services;


var builder = WebApplication.CreateBuilder(args);
var allowedOrigins = builder.Configuration["ALLOWED_ORIGINS"]?.Split(';') 
    ?? throw new Exception("ALLOWED_ORIGINS not configured");

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IAssistantService, OpenrouterAssistantService>();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Chat API"));

app.UseCors("FrontendPolicy");
app.MapHealthChecks("/health");
app.UseAuthorization();
app.MapControllers();
app.Run();