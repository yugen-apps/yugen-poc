namespace Poc.WorkerService.QueueService
{
    public sealed class BackgroundJobOptions
    {
        public int Capacity { get; set; } = 100;
        public int MaxAttempts { get; set; } = 3;
        public int DrainTimeoutSeconds { get; set; } = 30;
    }

}