using Microsoft.Extensions.Logging;
using ReCron;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore31Demo
{
    class TestJob1 : CronWorkerService
    {
        public TestJob1(IWorkerConfig<TestJob1> config, ILogger<TestJob1> logger) : base(config.CronExpression, TimeZoneInfo.Local)
        {

        }
        protected override async Task WorkerProcess(CancellationToken stoppingToken)
        {
            await Task.Delay(5000);
            await Task.Run(() => Console.WriteLine("TestJob1\t" + DateTime.Now.ToString()));
        }
    }

    class TestJob2 : CronWorkerService
    {
        public TestJob2(IWorkerConfig<TestJob2> config, ILogger<TestJob2> logger) : base(config.CronExpression, TimeZoneInfo.Local)
        {

        }
        protected override async Task WorkerProcess(CancellationToken stoppingToken)
        {
            await Task.Run(() => Console.WriteLine("TestJob2\t" + DateTime.Now.ToString()));
        }
    }

    class TestJob3 : CronWorkerService
    {
        public TestJob3(IWorkerConfig<TestJob3> config, ILogger<TestJob3> logger) : base(config.CronExpression, TimeZoneInfo.Local)
        {

        }
        protected override async Task WorkerProcess(CancellationToken stoppingToken)
        {
            await Task.Run(() => Console.WriteLine("TestJob3\t" + DateTime.Now.ToString()));
        }
    }
}
