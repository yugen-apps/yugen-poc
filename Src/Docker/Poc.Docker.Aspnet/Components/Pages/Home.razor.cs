using Poc.Docker.Aspnet.Data;
using Poc.Docker.Aspnet.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Poc.Docker.Aspnet.Components.Pages;

public partial class Home
{
    private const double Mebi = 1024 * 1024;
    private const double Gibi = Mebi * 1024;

    private readonly MyDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly ILogger<Home> _logger;

    private string? Txt;
    private string hostName = Dns.GetHostName();
    private EnvironmentInfo env = new();
    private IPAddress[] ipList = [];
    private bool _processing;

    private string ConnectionString => _configuration?.GetConnectionString("MsSql") ?? string.Empty;

    private string MyVar1 => _configuration?.GetValue<string>("MyVar1") ?? string.Empty;
    private string MyVar2 => _configuration?.GetValue<string>("MyVar2:MyVar2") ?? string.Empty;
    private string MyVar3 => _configuration?.GetValue<string>("MyVar3:MyVar3") ?? string.Empty;

    private string MySecret1 => _configuration?.GetValue<string>("MySecret1") ?? string.Empty;
    private string MySecret2 => _configuration?.GetValue<string>("MySecret2:MySecret2") ?? string.Empty;
    private string MySecret3 => _configuration?.GetValue<string>("MySecret3:MySecret3") ?? string.Empty;

    private string Pwd1 => _configuration?.GetValue<string>("ASPNETCORE_Kestrel:Certificates:Default:Password") ?? string.Empty;
    private string Pwd2 => _configuration?.GetValue<string>("Kestrel:Certificates:Default:Password") ?? string.Empty;

    private string Path1 => _configuration?.GetValue<string>("ASPNETCORE_Kestrel:Certificates:Default:Path") ?? string.Empty;
    private string Path2 => _configuration?.GetValue<string>("Kestrel:Certificates:Default:Path") ?? string.Empty;

    public string CanConnectText { get; set; } = "???";

    public Home(
        MyDbContext dbContext,
        IConfiguration configuration,
        ILogger<Home> logger)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task OnInitializedAsync()
    {
        ipList = await System.Net.Dns.GetHostAddressesAsync(hostName);

        // TestRead();

        // TestWrite();
    }


    private async Task CanConnectButtonOnClick()
    {
        _processing = true;
        await Task.Delay(500);
        CanConnectText = $"{CanConnect()}";
        _processing = false;

    }

    private bool CanConnect() => _dbContext.Database.CanConnect();

    private string GetInBestUnit(long size)
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
            Txt = System.IO.File.ReadAllText(System.IO.Path.Combine("/volume_dir", "test.txt"));
        }
        catch (Exception ex)
        {
            Txt = ex.Message;
        }
    }

    private void TestWrite()
    {
        try
        {
            System.IO.File.WriteAllText(System.IO.Path.Combine("/volume_dir", "test.txt"), "Hello World");
        }
        catch (Exception ex)
        {
            Txt = ex.Message;
        }
    }
}
