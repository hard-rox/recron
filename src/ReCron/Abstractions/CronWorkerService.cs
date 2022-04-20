using Cronos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ReCron
{
    public abstract class CronWorkerService : BackgroundService
    {
        private readonly CronExpression _expression;
        private readonly TimeZoneInfo _timeZoneInfo;
        private readonly ILogger _logger;

        protected CronWorkerService(string cronExpression, TimeZoneInfo timeZoneInfo, ILogger logger = null)
        {
            _expression = CronExpression.Parse(cronExpression);
            _timeZoneInfo = timeZoneInfo;
            _logger = logger; ///TODO: assign default logger in case of null...
        }

        protected abstract Task WorkerProcess(CancellationToken stoppingToken);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTimeOffset.Now;
                _logger.LogInformation("Worker started at {StartTime}", now);
                var next = _expression.GetNextOccurrence(now, _timeZoneInfo);
                _logger.LogInformation("Got next occurrence at {NextOccurrence}", next);
                if (next.HasValue)
                {
                    var delay = next.Value - DateTimeOffset.Now;
                    _logger.LogInformation("Got delay TimeSpan of {DelayTimespan}", delay);
                    await Task.Delay((int)delay.TotalMilliseconds, stoppingToken);
                    _logger.LogInformation("Worker Process started at {WorkerProcessStartedAt}", DateTimeOffset.Now);
                    await WorkerProcess(stoppingToken);
                    _logger.LogInformation("Worker Process ended at {WorkerProcessEndedAt}", DateTimeOffset.Now);
                }
                await Task.Delay(500, stoppingToken);
                _logger.LogInformation("Worker ended at {EndTime}", DateTimeOffset.Now);
            }
        }
    }
}
