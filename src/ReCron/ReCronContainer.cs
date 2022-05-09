namespace ReCron
{
    public static class ReCronContainer
    {
        private static List<Worker> _container = new List<Worker>();

        internal static void AddWorker<T>() where T:class
        {
            _container.Add(new Worker(typeof(T).FullName));
        }

        public static List<Worker> GetWorkers()
        {
            return _container;
        }

        public static void StopWorker(string name)
        {
            var worker = _container.FirstOrDefault(x => x.Name == name);
            if (worker == null) throw new ArgumentException("No worker found with supplied workerId");
            worker.SetStatus(WorkerStatus.Stopped);
        }

        public static void StartWorker(string name)
        {
            var worker = _container.FirstOrDefault(x => x.Name == name);
            if (worker == null) throw new ArgumentException("No worker found with supplied workerId");
            worker.SetStatus(WorkerStatus.Running);
        }
    }
}
