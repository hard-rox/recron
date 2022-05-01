using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ReCron.ConsoleTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var sp = new ServiceCollection()
                .AddCronWorker<TestJob>(config =>
                {
                    config.CronExpression = "*/10 * * * * *";
                    config.TimeZoneInfo = TimeZoneInfo.Local;
                });
        }
    }

    class TestJob : CronWorkerService
    {
        public TestJob(IWorkerConfig<TestJob> config) : base(config.CronExpression, TimeZoneInfo.Local)
        {

        }
        protected override async Task WorkerProcess(CancellationToken stoppingToken)
        {
            await Task.Run(() => Console.WriteLine(DateTime.Now.ToString()));
        }
    }
}
