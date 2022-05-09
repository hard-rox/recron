using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using ReCron;
using System;

namespace NetCore31Demo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCronWorker<TestJob1>(config =>
            {
                config.CronExpression = "*/1 * * * * *";
                config.TimeZoneInfo = TimeZoneInfo.Local;
            }).AddCronWorker<TestJob2>(config =>
            {
                config.CronExpression = "*/3 * * * * *";
                config.TimeZoneInfo = TimeZoneInfo.Local;
            }).AddCronWorker<TestJob3>(config =>
            {
                config.CronExpression = "*/5 * * * * *";
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
                endpoints.MapGet("/workers", async context =>
                {
                    var workers = ReCronContainer.GetWorkers();
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(workers));
                });

                endpoints.MapDelete("/workers/{name}", async context =>
                {
                    var name = context.Request.RouteValues["name"].ToString();
                    ReCronContainer.StopWorker(name);
                    context.Response.StatusCode = 200;
                });

                endpoints.MapPut("/workers/{name}", async context =>
                {
                    var name = context.Request.RouteValues["name"].ToString();
                    ReCronContainer.StartWorker(name);
                    context.Response.StatusCode = 200;
                });
            });
        }
    }
}
