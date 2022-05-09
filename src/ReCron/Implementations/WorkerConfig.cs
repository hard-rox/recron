namespace ReCron.Implementations
{
    internal class WorkerConfig<T> : IWorkerConfig<T> where T : CronWorkerService
    {
        public string CronExpression { get; set; }
        public TimeZoneInfo TimeZoneInfo { get; set; } = TimeZoneInfo.Local;
    }
}
