using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
// using Yugen.Home.Knx.Data;
namespace Yugen.Common.Blazor.Services;
//using Yugen.Common.Blazor.Account.Services;

public class InfoService : IInfoService
{
    private const double Mebi = 1024 * 1024;
    private const double Gibi = Mebi * 1024;

    //private readonly AuthService _authService;
    private readonly IConfiguration _configuration;
    // private readonly ApplicationDbContext _dbContext;
    private readonly IWebHostEnvironment _env;

    private readonly string _hostName = Dns.GetHostName();
    private readonly EnvironmentInfo _envInfo = new();
    private IPAddress[] _ipList = [];
    private string _txt;

    public InfoService(
        //AuthService authService,
        IConfiguration configuration,
        // ApplicationDbContext dbContext,
        IWebHostEnvironment env)
    {
        //_authService = authService;
        _configuration = configuration;
        // _dbContext = dbContext;
        _env = env;
    }

    public async Task<Dictionary<string, string>> GetAsync()
    {
        //var currentUser = await _authService.GetCurrentUserAsync();

        _ipList = await Dns.GetHostAddressesAsync(_hostName);

        //TestRead();

        //TestWrite();

        var List = new Dictionary<string, string>
        {
            { "Version", Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version ?? string.Empty },
            { "Environment", _configuration?["ASPNETCORE_ENVIRONMENT"] ?? string.Empty },
            //{ "ConfigurationConnectionString", currentUser.IsAuthenticated ? _configuration?.GetConnectionString("AZURE_SQL_CONNECTIONSTRING") ?? string.Empty : string.Empty},
            //{ "EnvConnectionString", currentUser.IsAuthenticated ? Environment.GetEnvironmentVariable("SQLAZURECONNSTR_AZURE_SQL_CONNECTIONSTRING") ?? string.Empty : string.Empty},
            // { "CanConnect", _dbContext.Database.CanConnect().ToString() },
            { "IsDevelopment", _env.IsDevelopment().ToString() },
            { ".NET version", RuntimeInformation.FrameworkDescription },
            { "Operating system", RuntimeInformation.OSDescription },
            { "Processor architecture", RuntimeInformation.OSArchitecture.ToString() },
            { "CPU cores", Environment.ProcessorCount.ToString() },
            { "Containerized", Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") is null ? "false" : "true" },
            { "User", Environment.UserName },
            { "cgroup memory limit", $"{_envInfo.MemoryLimit} {GetInBestUnit(_envInfo.MemoryLimit)}" },
            { "cgroup memory usage", $"{_envInfo.MemoryUsage} {GetInBestUnit(_envInfo.MemoryUsage)}" },
            { "Memory, total available GC memory", $"{_envInfo.TotalAvailableMemoryBytes} {GetInBestUnit(_envInfo.TotalAvailableMemoryBytes)}" },
            { "Host name", _hostName },
            { "Server IP address", string.Join(Environment.NewLine, _ipList) }
            //{ "Txt", _txt  }
        };

        return List;
    }

    // public bool CanConnect() => _dbContext.Database.CanConnect();

    private static string GetInBestUnit(long size)
    {
        if (size < Mebi)
        {
            return $"{size} bytes";
        }
        else if (size < Gibi)
        {
            double mebibytes = size / Mebi;
            return $"{mebibytes:N2} MiB";
        }
        else
        {
            double gibibytes = size / Gibi;
            return $"{gibibytes:N2} GiB";
        }
    }

    private void TestRead()
    {
        try
        {
            _txt = File.ReadAllText(System.IO.Path.Combine("/volume_dir", "test.txt"));
        }
        catch (Exception ex)
        {
            _txt = ex.Message;
        }
    }

    private void TestWrite()
    {
        try
        {
            File.WriteAllText(System.IO.Path.Combine("/volume_dir", "test.txt"), "Hello World");
        }
        catch (Exception ex)
        {
            _txt = ex.Message;
        }
    }
}

public readonly struct EnvironmentInfo
{
    public EnvironmentInfo()
    {
        GCMemoryInfo gcInfo = GC.GetGCMemoryInfo();
        TotalAvailableMemoryBytes = gcInfo.TotalAvailableMemoryBytes;

        if (!OperatingSystem.IsLinux())
        {
            return;
        }

        string[] memoryLimitPaths =
        [
            "/sys/fs/cgroup/memory.max",
            "/sys/fs/cgroup/memory.high",
            "/sys/fs/cgroup/memory.low",
            "/sys/fs/cgroup/memory/memory.limit_in_bytes",
        ];

        string[] currentMemoryPaths =
        [
            "/sys/fs/cgroup/memory.current",
            "/sys/fs/cgroup/memory/memory.usage_in_bytes",
        ];

        MemoryLimit = GetBestValue(memoryLimitPaths);
        MemoryUsage = GetBestValue(currentMemoryPaths);
    }

    public string RuntimeVersion => RuntimeInformation.FrameworkDescription;
    public string OSVersion => RuntimeInformation.OSDescription;
    public string OSArchitecture => RuntimeInformation.OSArchitecture.ToString();
    public string User => Environment.UserName;
    public int ProcessorCount => Environment.ProcessorCount;
    public long TotalAvailableMemoryBytes { get; }
    public long MemoryLimit { get; }
    public long MemoryUsage { get; }
    public string HostName => Dns.GetHostName();

    private static long GetBestValue(string[] paths)
    {
        foreach (string path in paths)
        {
            if (Path.Exists(path) &&
                long.TryParse(File.ReadAllText(path), out long result))
            {
                return result;
            }
        }

        return 0;
    }
}

