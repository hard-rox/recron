using Net6Demo;
using ReCron;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCronWorker<TestJob1>(config =>
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

var app = builder.Build();

app.MapGet("/workers", () => ReCronContainer.GetWorkers());
app.MapDelete("/workers/{name}", (string name) => ReCronContainer.StopWorker(name));
app.MapPut("/workers/{name}", (string name) => ReCronContainer.StartWorker(name));

app.Run();
