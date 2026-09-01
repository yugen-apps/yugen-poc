using Microsoft.Extensions.Configuration;
using Poc.Common;
using Poc.Common.Data;
using Poc.Common.Web;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Poc.Ef.Aspnet.Components.Pages;

public partial class Info
{
    private static JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web) { WriteIndented = true };

    private readonly SystemInfoService _systemInfoService;
    private readonly WebHostInfoService _webHostInfoService;
    private readonly IBaseRepository _baseRepository;
    private readonly IConfiguration _configuration;

    private Dictionary<string, string> _systemInfos = [];
    private Dictionary<string, string> _webHostEnvironmentInfos = [];
    private Dictionary<string, string> _configurationKeyValues = [];

    public bool Processing { get; set; }

    public string CanConnectText { get; set; } = "???";

    public Info(
        SystemInfoService systemInfoService,
        WebHostInfoService webHostInfoService,
        IBaseRepository baseRepository,
        IConfiguration configuration)
    {
        _systemInfoService = systemInfoService;
        _webHostInfoService = webHostInfoService;
        _baseRepository = baseRepository;
        _configuration = configuration;
    }

    protected override async Task OnInitializedAsync()
    {
        _systemInfos = await _systemInfoService.InitializedAsync();

        _webHostEnvironmentInfos = _webHostInfoService.Initialize();

        _configurationKeyValues = _systemInfoService.AllConfigurationKeyValues(_configuration);
    }

    //private readonly IConfiguration _configuration;

    //private string ConnectionString => _configuration?.GetConnectionString("MsSql") ?? string.Empty;

    //private string MyVar1 => _configuration?.GetValue<string>("MyVar1") ?? string.Empty;
    //private string MyVar2 => _configuration?.GetValue<string>("MyVar2:MyVar2") ?? string.Empty;
    //private string MyVar3 => _configuration?.GetValue<string>("MyVar3:MyVar3") ?? string.Empty;

    //private string MySecret1 => _configuration?.GetValue<string>("MySecret1") ?? string.Empty;
    //private string MySecret2 => _configuration?.GetValue<string>("MySecret2:MySecret2") ?? string.Empty;
    //private string MySecret3 => _configuration?.GetValue<string>("MySecret3:MySecret3") ?? string.Empty;

    //private string Pwd1 => _configuration?.GetValue<string>("ASPNETCORE_Kestrel:Certificates:Default:Password") ?? string.Empty;
    //private string Pwd2 => _configuration?.GetValue<string>("Kestrel:Certificates:Default:Password") ?? string.Empty;

    //private string Path1 => _configuration?.GetValue<string>("ASPNETCORE_Kestrel:Certificates:Default:Path") ?? string.Empty;
    //private string Path2 => _configuration?.GetValue<string>("Kestrel:Certificates:Default:Path") ?? string.Empty;

    private async Task CanConnectButtonOnClick()
    {
        Processing = true;
        await Task.Delay(500);
        CanConnectText = $"{_baseRepository.CanConnect()}";
        Processing = false;
    }
}
