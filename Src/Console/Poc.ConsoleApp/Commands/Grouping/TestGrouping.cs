using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.ConsoleApp.Commands.Grouping;

public class GroupingCommand : AsyncCommand
{
    private readonly IAnsiConsole _console;

    public GroupingCommand(
        IAnsiConsole console)
    {
        _console = console;
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
    {
        _console.WriteLine("Test Grouped Titles...");
        var result = await TestGroupedTitles();
        foreach (var item in result)
        {
            _console.WriteLine($"{item.Id} {item.CurrencyId} {item.LicenseModelId}");
        }

        _console.WriteLine("Test Unique Titles...");
        result = await TestUniqueTitles();
        foreach (var item in result)
        {
            _console.WriteLine($"{item.Id} {item.CurrencyId} {item.LicenseModelId}");
        }

        _console.Prompt(
            new TextPrompt<string>("...")
                .AllowEmpty());

        return 0;
    }

    private static readonly List<CatalogueTitle> TitlesResult =
    [
        new(1, "0", 0, new DateTime(2026,1,1)),
            new(1, "0", 0, new DateTime(2026,1,2)),

            new(0, "1", 0, new DateTime(2026,1,2)),
            new(0, "1", 0, new DateTime(2026,1,1)),

            new(0, "0", 1, new DateTime(2026,1,1)),
            new(0, "0", 1, new DateTime(2026,1,2)),

            new(1, "2", 3, new DateTime(2026,1,1)),

            new(4, "5", 6, new DateTime(2026,1,1)),
        ];

    public async Task<List<CatalogueTitle>> TestGroupedTitles()
    {
        var groupedTitles = TitlesResult
            .GroupBy(x => new { x.Isbn13, x.CurrencyId, x.LicenseModelId })
            .Select(g => g.OrderByDescending(y => y.DateModified).First())
            .ToList();

        return groupedTitles;
    }

    public async Task<List<CatalogueTitle>> TestUniqueTitles()
    {
        List<CatalogueTitle> uniqueTitles = [];

        var groupedTitles = TitlesResult
            .GroupBy(x => new { x.Isbn13, x.CurrencyId, x.LicenseModelId })
            .ToList();

        foreach (var titles in groupedTitles)
        {
            var orderedTitles = titles.OrderByDescending(y => y.DateModified).ToList();
            if (orderedTitles.Count < 1)
            {
                continue;
            }

            uniqueTitles.Add(titles.First());
            if (orderedTitles.Count < 2)
            {
                continue;
            }

            for (var i = 1; i < orderedTitles.Count; i++)
            {
                _console.WriteLine($"Warning: {orderedTitles[i]}");
            }
        }

        return uniqueTitles;
    }
}