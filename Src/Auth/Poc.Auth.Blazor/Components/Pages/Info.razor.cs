using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Graph.Models.ExternalConnectors;
using Poc.Common;
using Poc.Common.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Poc.Auth.Blazor.Components.Pages;

public partial class Info
{
	private static JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web) { WriteIndented = true };

	private readonly SystemInfoService _systemInfoService;
	private readonly WebHostInfoService _webHostInfoService;
	private readonly IConfiguration _configuration;

	private Dictionary<string, string> _systemInfos = [];
	private Dictionary<string, string> _webHostEnvironmentInfos = [];
	private Dictionary<string, string> _configurationKeyValues = [];

	public Info(
		SystemInfoService systemInfoService,
		WebHostInfoService webHostInfoService,
		IConfiguration configuration)
	{
		_systemInfoService = systemInfoService;
		_webHostInfoService = webHostInfoService;
		_configuration = configuration;
	}

	protected override async Task OnInitializedAsync()
	{
		_systemInfos = await _systemInfoService.InitializedAsync();

		_webHostEnvironmentInfos = _webHostInfoService.Initialize();

		_configurationKeyValues = _systemInfoService.AllConfigurationKeyValues(_configuration);
	}
}
