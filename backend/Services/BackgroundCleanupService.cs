using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using IntelliBot.Core.Services;

namespace IntelliBot.Core.Services
{
    public class BackgroundCleanupService : BackgroundService
    {
        private readonly IAssistantService _assistantService;
        private readonly ILogger<BackgroundCleanupService> _logger;
        private readonly TimeSpan _cleanupInterval = TimeSpan.FromMinutes(5);

        public BackgroundCleanupService(
            IAssistantService assistantService,
            ILogger<BackgroundCleanupService> logger)
        {
            _assistantService = assistantService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Conversation cleanup service is starting");
            
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _assistantService.CleanupExpiredConversations();
                    _logger.LogInformation("Expired conversations cleaned up");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error cleaning up conversations");
                }
                
                await Task.Delay(_cleanupInterval, stoppingToken);
            }
            
            _logger.LogInformation("Conversation cleanup service is stopping");
        }
    }
}