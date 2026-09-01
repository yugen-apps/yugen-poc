using Poc.Ef.Domain.Shared;
using Spectre.Console;
using System.Text.Json;

namespace Poc.Ef.WorkerService.Helpers;

public static class LogHelper
{
    public static void Log<T>(IAnsiConsole console, Result<T> result)
    {
        console.WriteLine($"Succeeded: {result.Succeeded} Data: {JsonSerializer.Serialize(result.Data)}");
    }
}