using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

internal class Program
{
    public class CatalogueTitle
    {
        public CatalogueTitle(
            int currencyId,
            string isbn13,
            int licenseModelId,
            DateTime dateModified)
        {
            Id = Guid.NewGuid();
            CurrencyId = currencyId;
            Isbn13 = isbn13;
            LicenseModelId = licenseModelId;
            DateModified = dateModified;
        }

        public Guid Id { get; set; }
        public int CurrencyId { get; set; }
        public string Isbn13 { get; set; }
        public int LicenseModelId { get; set; }
        public DateTime DateModified { get; set; }
    }

    private static readonly List<CatalogueTitle> titlesResult =
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

    static async Task Main()
    {
        await Init2();
    }

    static async Task Init()
    {
        var titles = titlesResult
            .GroupBy(x => new { x.Isbn13, x.CurrencyId, x.LicenseModelId })
            .Select(g => g.OrderByDescending(y => y.DateModified).First())
            .ToList();
    }

    static async Task Init2()
    {
        List<CatalogueTitle> uniqueTitles = [];

        var groupedTitles = titlesResult
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

            for (int i = 1; i < orderedTitles.Count; i++)
            {
                // log warning for orderedTitles[i];
            }
        }
    }

    //private static List<string> list = new List<string>();

    //static async Task Init()
    //{
    //    Queue<int> a = new Queue<int>(2);

    //    LinkedList<int> b = new LinkedList<int>();

    //    for (int i = 0; i < 5; i++)
    //    {
    //        a.Enqueue(i);
    //    }

    //    _ = Task.Run(async () =>
    //    {
    //        for (int i = 0; i < 10; i++)
    //        {
    //            list.Add(i.ToString());
    //            Console.WriteLine($"add {i}");
    //            await Task.Delay(100);
    //        }
    //    });

    //    await Task.Delay(500);

    //    _ = Task.Run(() =>
    //    {
    //        foreach (var i in list.ToArray())
    //        {
    //            list.Remove(i);
    //            Console.WriteLine($"remove {i}");
    //        }
    //    });

    //    Console.ReadLine();
    //}

    //    private static AutoResetEvent event_1 = new AutoResetEvent(false);

    //    static async Task Init()
    //    {
    //        Console.WriteLine("Press Enter to create three threads and start them.\r\n" +
    //                          "The threads wait on AutoResetEvent #1, which was created\r\n" +
    //                          "in the signaled state, so the first thread is released.\r\n" +
    //                          "This puts AutoResetEvent #1 into the unsignaled state.");
    //        Console.ReadLine();

    //        for (int i = 1; i < 4; i++)
    //        {
    //            Task t = new Task(ThreadProc);
    //            t.Start();
    //        }
    //        Thread.Sleep(250);

    //        for (int i = 0; i < 3; i++)
    //        {
    //            Console.WriteLine("Press Enter to release another thread.");
    //            Console.ReadLine();
    //            event_1.Set();
    //            Thread.Sleep(250);
    //        }

    //    }

    //    static void ThreadProc()
    //    {
    //        Console.WriteLine("{0} waits on AutoResetEvent #1.");
    //        if (event_1.WaitOne())
    //        {
    //            Console.WriteLine("{0} is released from AutoResetEvent #1.");
    //            Console.WriteLine("{0} ends.");
    //        }
    //        else
    //        {
    //            Console.WriteLine("{0} time out.");
    //        }
    //    }
}