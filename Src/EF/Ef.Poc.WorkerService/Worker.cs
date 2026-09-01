using Ef.Poc.Application.Contracts.Services;
using Ef.Poc.Domain.Shared;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Ef.Poc.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<Worker> _logger;

        public Worker(
            ICategoryService categoryService,
            ILogger<Worker> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            while (!cancellationToken.IsCancellationRequested)
            {
                await MenuAsync(cancellationToken);

                // await Task.Delay(1000, cancellationToken);
            }
        }

        private async Task MenuAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("1 Add");
            Console.WriteLine("2 GetAll");
            Console.WriteLine("3 Get");
            Console.WriteLine("4 Update");
            Console.WriteLine("5 Delete");
            var menu = Console.ReadLine();

            switch (menu)
            {
                case "1":
                    await Add(cancellationToken);
                    break;
                case "2":
                    await GetAll(cancellationToken);
                    break;
                case "3":
                    await Get(cancellationToken);
                    break;
                case "4":
                    await Update(cancellationToken);
                    break;
                case "5":
                    await Delete(cancellationToken);
                    break;
            }
        }

        private async Task Add(CancellationToken cancellationToken)
        {
            Console.WriteLine("Add...");

            Console.WriteLine("Insert Title:");
            var title = Console.ReadLine() ?? "";

            var result = await _categoryService.AddAsync(new() { Title = title }, cancellationToken);

            Log(result);
        }

        private async Task GetAll(CancellationToken cancellationToken)
        {
            Console.WriteLine("GetAll...");

            var result = await _categoryService.GetAllAsync();

            Log(result);
        }

        private async Task Get(CancellationToken cancellationToken)
        {
            Console.WriteLine("Get...");


            Console.WriteLine("Insert Id:");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                id = 1;
            }

            var result = await _categoryService.GetAsync(id);

            Log(result);
        }

        private async Task Update(CancellationToken cancellationToken)
        {
            Console.WriteLine("Update...");

            Console.WriteLine("Insert Id:");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                id = 1;
            }

            Console.WriteLine("Insert Title:");
            var title = Console.ReadLine() ?? "";

            var result = await _categoryService.UpdateAsync(id, new() { Title = title }, cancellationToken);

            Log(result);
        }

        private async Task Delete(CancellationToken cancellationToken)
        {
            Console.WriteLine("Delete...");

            Console.WriteLine("Insert Id:");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                id = 1;
            }

            var result = await _categoryService.DeleteAsync(id, cancellationToken);

            Log(result);
        }

        private void Log<T>(Result<T> result)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Succeeded: {Succeeded} Data: {Data}", result.Succeeded, JsonSerializer.Serialize(result.Data));
            }
        }
    }
}
