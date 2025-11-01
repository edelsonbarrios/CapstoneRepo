using PharmacyBackend.Contracts.ServiceContracts.DeleteInterface;
using PharmacyBackend.Contracts.ServiceContracts.PostInterface;

namespace PharmacyBackend.Service.BackgroundServices
{
    /// <summary>
    /// Background service that automatically performs database backups based on configured schedule
    /// </summary>
    public class BackupBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BackupBackgroundService> _logger;
        private readonly IConfiguration _config;
        private Timer? _timer;

        public BackupBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<BackupBackgroundService> logger,
            IConfiguration config)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Backup Background Service is starting");

            // Check if auto backup is enabled
            var autoBackupEnabled = _config.GetValue<bool>("BackupSettings:AutoBackupEnabled", true);

            if (!autoBackupEnabled)
            {
                _logger.LogInformation("Auto backup is disabled in configuration");
                return;
            }

            // Wait for application to fully start
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.Now;
                    var nextBackupTime = CalculateNextBackupTime(now);
                    var delay = nextBackupTime - now;

                    _logger.LogInformation($"Next automated backup scheduled at: {nextBackupTime:yyyy-MM-dd HH:mm:ss}");
                    _logger.LogInformation($"Waiting {delay.TotalHours:F2} hours until next backup");

                    // Wait until the scheduled time
                    await Task.Delay(delay, stoppingToken);

                    if (!stoppingToken.IsCancellationRequested)
                    {
                        await PerformBackupAsync();
                    }
                }
                catch (TaskCanceledException)
                {
                    _logger.LogInformation("Backup service is stopping");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in backup service main loop");
                    // Wait 1 hour before retrying if there's an error
                    await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
                }
            }
        }

        private DateTime CalculateNextBackupTime(DateTime now)
        {
            var backupHour = _config.GetValue<int>("BackupSettings:AutoBackupHour", 2);
            var backupMinute = _config.GetValue<int>("BackupSettings:AutoBackupMinute", 0);

            // Calculate today's backup time
            var todayBackupTime = new DateTime(
                now.Year,
                now.Month,
                now.Day,
                backupHour,
                backupMinute,
                0
            );

            // If today's backup time has passed, schedule for tomorrow
            if (now >= todayBackupTime)
            {
                return todayBackupTime.AddDays(1);
            }

            return todayBackupTime;
        }

        private async Task PerformBackupAsync()
        {
            using var scope = _serviceProvider.CreateScope();

            try
            {
                var postService = scope.ServiceProvider.GetRequiredService<IPostServiceContracts>();
                var deleteService = scope.ServiceProvider.GetRequiredService<IDeleteServiceContracts>();

                _logger.LogInformation("Starting automated backup process");

                // Perform the automated backup
                await postService.PerformAutomatedBackupAsync();

                _logger.LogInformation("Automated backup completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to perform automated backup");
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Backup Background Service is stopping");
            _timer?.Dispose();
            await base.StopAsync(cancellationToken);
        }
    }
}
