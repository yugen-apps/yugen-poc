using Poc.Common.Spectre.Models;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Poc.ConsoleApp.Commands;

public class TaskCommand : AsyncCommand
{
    public static CommandDefinition CommandDefinition => new("Task");

    private readonly IAnsiConsole _console;

    public TaskCommand(
        IAnsiConsole console)
    {
        _console = console;
    }
    protected override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
    {
        _console.WriteLine("Test1...");
        await Test1();

        _console.WriteLine("Test2...");
        await Test2();

        _console.Prompt(
            new TextPrompt<string>("...")
                .AllowEmpty());

        return 0;
    }

    private List<string> _list = new List<string>();

    public async Task Test1()
    {
        var a = new Queue<int>(2);

        var b = new LinkedList<int>();

        for (var i = 0; i < 5; i++)
        {
            a.Enqueue(i);
        }

        _ = Task.Run(async () =>
        {
            for (var i = 0; i < 10; i++)
            {
                _list.Add(i.ToString());
                _console.WriteLine($"add {i}");
                await Task.Delay(100);
            }
        });

        await Task.Delay(500);

        _ = Task.Run(() =>
        {
            foreach (var i in _list.ToArray())
            {
                _list.Remove(i);
                _console.WriteLine($"remove {i}");
            }
        });

        _console.Prompt(
            new TextPrompt<string>("...")
                .AllowEmpty());
    }

    private AutoResetEvent _event1 = new AutoResetEvent(false);

    public async Task Test2()
    {
        _console.WriteLine("Press Enter to create three threads and start them.\r\n" +
                          "The threads wait on AutoResetEvent #1, which was created\r\n" +
                          "in the signaled state, so the first thread is released.\r\n" +
                          "This puts AutoResetEvent #1 into the unsignaled state.");
        _console.Prompt(
            new TextPrompt<string>("...")
                .AllowEmpty());

        for (var i = 1; i < 4; i++)
        {
            var t = new Task(ThreadProc);
            t.Start();
        }
        Thread.Sleep(250);

        for (var i = 0; i < 3; i++)
        {
            _console.WriteLine("Press Enter to release another thread.");

            _console.Prompt(
                new TextPrompt<string>("...")
                    .AllowEmpty());

            _event1.Set();
            Thread.Sleep(250);
        }

    }

    private void ThreadProc()
    {
        _console.WriteLine("{0} waits on AutoResetEvent #1.");
        if (_event1.WaitOne())
        {
            _console.WriteLine("{0} is released from AutoResetEvent #1.");
            _console.WriteLine("{0} ends.");
        }
        else
        {
            _console.WriteLine("{0} time out.");
        }
    }
}