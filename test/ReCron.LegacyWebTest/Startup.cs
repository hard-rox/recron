using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ReCron.LegacyWebTest
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCronWorker<TestJob1>(config =>
            {
                config.CronExpression = "*/2 * * * * *";
                config.TimeZoneInfo = TimeZoneInfo.Local;
            });

            services.AddCronWorker<TestJob2>(config =>
            {
                config.CronExpression = "*/10 * * * * *";
                config.TimeZoneInfo = TimeZoneInfo.Local;
            });

            services.AddCronWorker<TestJob3>(config =>
            {
                config.CronExpression = "*/15 * * * * *";
                config.TimeZoneInfo = TimeZoneInfo.Local;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    var workers = ReCronContainer.GetWorkers();
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(workers));
                });

                endpoints.MapDelete("/{workerId}", async context =>
                {
                    var workerId = context.Request.RouteValues["workerId"].ToString();
                    ReCronContainer.StopWorker(workerId);
                    context.Response.StatusCode = 200;
                });
            });
        }
    }

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
