using Microsoft.Extensions.DependencyInjection;
using ReCron.Implementations;

namespace ReCron
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddCronWorker<T>(this IServiceCollection services, Action<IWorkerConfig<T>> config) where T : CronWorkerService
        {
            if(config == null)
                throw new ArgumentNullException(nameof(config), @"Please provide Worker Configurations.");

            var workerConfig = new WorkerConfig<T>();
            config.Invoke(workerConfig);

            if (string.IsNullOrWhiteSpace(workerConfig.CronExpression))
                throw new ArgumentNullException(nameof(workerConfig.CronExpression), @"Empty Cron Expression is not allowed.");

            services.AddSingleton<IWorkerConfig<T>>(workerConfig);
            services.AddHostedService<T>();

            return services;
        }
    }
}
