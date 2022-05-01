namespace ReCron
{
    public class Worker
    {
        public string Name { get; private set; }
        public WorkerStatus Status { get; private set; }

        internal Worker(string name)
        {
            Name = name;
            Status = WorkerStatus.Added;
        }

        internal void SetStatus(WorkerStatus status)
        {
            Status = status;
        }
    }
}
