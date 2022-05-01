using Cronos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ReCron
{
    public abstract class CronWorkerService : BackgroundService
    {
        private readonly CronExpression _expression;
        private readonly TimeZoneInfo _timeZoneInfo;
        private readonly ILogger? _logger;

        protected CronWorkerService(string cronExpression, TimeZoneInfo timeZoneInfo, ILogger? logger = null)
        {
            var cronExpLength = cronExpression.Split(' ').Length;
            if (cronExpLength < 5 || cronExpLength > 6) throw new ArgumentException("Invalid Cron Expression", nameof(cronExpression));

            _expression = CronExpression.Parse(cronExpression, cronExpLength > 5 ? CronFormat.IncludeSeconds : CronFormat.Standard);
            _timeZoneInfo = timeZoneInfo;
            _logger = logger;
        }

        protected abstract Task WorkerProcess(CancellationToken stoppingToken);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var worker = ReCronContainer.GetWorkers().FirstOrDefault(x => x.Name == this.GetType().FullName);
                if (worker.Status == WorkerStatus.Added) worker.SetStatus(WorkerStatus.Running);
                else if(worker.Status != WorkerStatus.Stopped)
                {
                    var now = DateTimeOffset.Now;
                    if (_logger != null) _logger.LogInformation("Worker started at {StartTime}", now);
                    var next = _expression.GetNextOccurrence(now, _timeZoneInfo);
                    if (_logger != null) _logger.LogInformation("Got next occurrence at {NextOccurrence}", next);
                    if (next.HasValue)
                    {
                        var delay = next.Value - DateTimeOffset.Now;
                        if (_logger != null) _logger.LogInformation("Got delay TimeSpan of {DelayTimespan}", delay);
                        await Task.Delay((int)delay.TotalMilliseconds, stoppingToken);
                        if (_logger != null) _logger.LogInformation("Worker Process started at {WorkerProcessStartedAt}", DateTimeOffset.Now);
                        await WorkerProcess(stoppingToken);
                        if (_logger != null) _logger.LogInformation("Worker Process ended at {WorkerProcessEndedAt}", DateTimeOffset.Now);
                    }
                    await Task.Delay(500, stoppingToken);
                    if (_logger != null) _logger.LogInformation("Worker ended at {EndTime}", DateTimeOffset.Now);
                }
            }
        }
    }
}
