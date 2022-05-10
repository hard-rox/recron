# ReCron
[![NuGet](https://img.shields.io/nuget/v/ReCron.svg)](https://www.nuget.org/packages/ReCron)
![Github Workflow](https://img.shields.io/github/workflow/status/hard-rox/recron/nuget%20publish)
![Contributors](https://img.shields.io/github/contributors/hard-rox/recron)
![Downloads](https://img.shields.io/nuget/dt/ReCron)

**ReCron** is a fully-featured .NET library for creating and running Cron/Scheduled jobs using background service for **.NET Core 3.1**, **.NET 5** and upwords.

## Compatibility

|.NET Version|Supported|Remark|
|------------|---------|------|
|.NET 6|✅||
|.NET 5|✅||
|.NET Core 3.1|✅||
|.NET Core (1.0 - 3.0)|❌|This library is targeting .NET Core 3.1, .NET 5 ++
|.NET Framework (1.0 - 4.8)|❌|This library is targeting .NET Core 3.1, .NET 5 ++

## Installation

Package Manager
```PM
Install-Package ReCron -Version 0.2.1
```
.NET CLI
```
dotnet add package ReCron --version 0.2.1
```
Package Reference 
```xml
<PackageReference Include="ReCron" Version="0.2.1" />
```

### Usage

1. Create a job class by inheriting ```CronWorkerService``` class and implement ```WorkerProcess``` method.
```C#
class TestJob : CronWorkerService
{
    public TestJob(IWorkerConfig<TestJob1> config, ILogger<TestJob1> logger) : base(config.CronExpression, TimeZoneInfo.Local)
    {

    }
    protected override async Task WorkerProcess(CancellationToken stoppingToken)
    {
        await Task.Run(() => Console.WriteLine("TestJob\t" + DateTime.Now.ToString()));
    }
}
```
2. Inject your job with cron expression and ```TimeZoneInfo```
```C#
services.AddCronWorker<TestJob>(config =>
{
    config.CronExpression = "*/1 * * * * *";
    config.TimeZoneInfo = TimeZoneInfo.Local;
});
```
For working examples see ```/demos``` directory.


## Contribution

## Authors

* **Rasedur Rahman Roxy** - *Planning and Development* - [Twitter](https://twitter.com/roxyxmw?lang=en)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

* Hat tip to anyone whose code was used
