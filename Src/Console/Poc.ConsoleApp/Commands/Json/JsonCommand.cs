using Poc.Shared.Json;
using Spectre.Console;
using Spectre.Console.Cli;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.ConsoleApp.Commands.Json;

public class JsonCommand : AsyncCommand
{
    private readonly IAnsiConsole _console;

    public JsonCommand(
        IAnsiConsole console)
    {
        _console = console;
    }

    public override async Task<int> ExecuteAsync(CommandContext commandContext, CancellationToken cancellationToken)
    {
        //var cwd = Directory.GetCurrentDirectory();
        var cwd = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
        if (cwd == null)
        {
            _console.Prompt(
                new TextPrompt<string>("...")
                    .AllowEmpty());

            return 0;
        }

        var filePath = Path.Combine(cwd, "Json", "test.json");
        var result = await JsonHelper.ReadTextAsync(filePath);
        _console.WriteLine($"result...{result}");

        var result2 = await JsonHelper.Get<TestJson>(filePath);
        if (result2 != null)
        {
            var testString = result2.TestString.GetString();
            var testInt = result2.TestInt.GetInt32();
            var testArray = result2.TestArray.EnumerateArray();
            var stringArray = JsonSerializer.Deserialize<string[]>(result2.StringArrayAsString ?? "", JsonHelper.JsonSerializerOptions);
            var intArray = ParseGroupIds(result2.IntArrayAsString ?? string.Empty);

            var properties = result2.Properties;
            var bookidJsonElement = properties.TryGetPropertyOrNull("bookid");
            var bookidString = properties.TryGetString("bookid");
            var bookidIsString = properties.IsString("bookid");
            var bookidInt = properties.TryGetInt("bookid");
            var bookidIsInt = properties.TryGetString("bookid");

            var noteidJsonElement = properties.TryGetPropertyOrNull("noteid");
            var noteidString = properties.TryGetString("noteid");
            var noteidIsString = properties.IsString("noteid");
            var noteidInt = properties.TryGetInt("noteid");
            var noteidIsInt = properties.TryGetString("noteid");

            _console.WriteLine($"resul2...{result2.Name}");
        }

        var result3 = await JsonHelper.Set(result2);
        _console.WriteLine($"result3...{result3}");

        filePath = Path.Combine(cwd, "Json", "test2.json");
        var result4 = await JsonHelper.Get<TestJson>(filePath);
        if (result4 != null)
        {
            var testString = result4.TestString.GetString();
            var testInt = result4.TestInt.GetInt32();
            var testArray = result4.TestArray.EnumerateArray();
            var stringArray = JsonSerializer.Deserialize<string[]>(result4.StringArrayAsString ?? "[]", JsonHelper.JsonSerializerOptions);
            var intArray = ParseGroupIds(result4.IntArrayAsString ?? string.Empty);
            var properties = result4.Properties;

            _console.WriteLine($"resul4...{result4.Name}");
        }

        _console.Prompt(
            new TextPrompt<string>("...")
                .AllowEmpty());

        return 0;
    }

    private static int[] ParseGroupIds(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            return [];
        }

        try
        {
            return JsonSerializer.Deserialize<int[]>(s, JsonHelper.JsonSerializerOptions) ?? [];
        }
        catch
        {
            return [];
        }
    }
}