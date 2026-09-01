using System.Text.Json;
using Spectre.Console;
using Yugen.Common.Core.Models;

namespace Poc.Identity.WorkerService.Helpers;

public static class LogHelper
{
    public static void Log<T>(IAnsiConsole console, Result<T> result)
    {
        console.WriteLine($"Succeeded: {result.IsSuccess} Data: {JsonSerializer.Serialize(result.Value)}");
    }
}