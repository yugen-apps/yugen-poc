using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Poc.WorkerService.TaskQueueService;

namespace Poc.WorkerService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        //builder.Services.AddHostedService<TimerBasedService>();

        //builder.Services.Configure<BackgroundJobOptions>(builder.Configuration.GetSection("BackgroundJobs"));
        //builder.Services.AddSingleton<IMessageQueue<string>, MessageQueue<string>>();
        //builder.Services.AddHostedService<QueueProducerService>();
        //builder.Services.AddHostedService<QueueProcessorService>();

        builder.Services.AddSingleton<ITaskQueue>(_ =>
        {
            if (!int.TryParse(builder.Configuration["QueueCapacity"], out var queueCapacity))
            {
                queueCapacity = 100;
            }

            return new TaskQueue(queueCapacity);
        });
        builder.Services.AddHostedService<TaskQueueProducerService>();
        builder.Services.AddHostedService<TaskQueueProcessorService>();

        var host = builder.Build();
        host.Run();
    }
}
