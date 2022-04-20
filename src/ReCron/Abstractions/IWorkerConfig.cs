namespace ReCron
{
    public interface IWorkerConfig<T> where T : CronWorkerService
    {
        string CronExpression { get; set; }
        TimeZoneInfo TimeZoneInfo { get; set; }
    }
}
