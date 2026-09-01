using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Poc.Common;

public class SystemInfoService
{	
	private static JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web) { WriteIndented = true };

	private readonly ILogger<SystemInfoService> _logger;
	private readonly Dictionary<string, string> _list = [];	
	private readonly EnvironmentInfo _environmentInfo = new();

	public SystemInfoService(
		ILogger<SystemInfoService> logger)
	{
		_logger = logger;
	}

	public async Task<Dictionary<string, string>> InitializedAsync()
	{
		_list.Add(nameof(_environmentInfo.RuntimeVersion), _environmentInfo.RuntimeVersion);
		_list.Add(nameof(_environmentInfo.OSVersion), _environmentInfo.OSVersion);
		_list.Add(nameof(_environmentInfo.CpuArchitecture), _environmentInfo.CpuArchitecture);
		_list.Add(nameof(_environmentInfo.CpuCores), _environmentInfo.CpuCores.ToString());
		_list.Add(nameof(_environmentInfo.Containerized), _environmentInfo.Containerized.ToString());
		_list.Add(nameof(_environmentInfo.UserName), _environmentInfo.UserName);

		_list.Add(nameof(_environmentInfo.MemoryLimit), _environmentInfo.MemoryLimitString);
		_list.Add(nameof(_environmentInfo.MemoryUsage), _environmentInfo.MemoryUsageString);
		_list.Add(nameof(_environmentInfo.TotalAvailableMemoryBytesString), _environmentInfo.TotalAvailableMemoryBytesString);

		_list.Add(nameof(_environmentInfo.HostName), _environmentInfo.HostName);
		_list.Add(nameof(_environmentInfo.IpList), await _environmentInfo.IpListString());

		return _list;
	}

	public string TestRead()
	{
		try
		{
			return File.ReadAllText(Path.Combine("/volume_dir", "test.txt"));
		}
		catch (Exception ex)
		{
			return ex.Message;
		}
	}

	public string TestWrite()
	{
		try
		{
			File.WriteAllText(Path.Combine("/volume_dir", "test.txt"), "Hello World");
			return "OK";
		}
		catch (Exception ex)
		{
			return ex.Message;
		}
	}

	

	public Dictionary<string, string> AllConfigurationKeyValues(IConfiguration configuration)
	{
		var list = new Dictionary<string, string?>();
		foreach (var provider in ((IConfigurationRoot)configuration).Providers)
		{
			foreach (var key in GetFullKeyNames(provider, null, []).OrderBy(p => p))
			{
				if (provider.TryGet(key, out var value))
				{
					list.Add($"{provider} {key}", value ?? string.Empty);
				}
			}
		}
		return list;
	}

	private static HashSet<string> GetFullKeyNames(IConfigurationProvider provider, string? rootKey, HashSet<string> initialKeys)
	{
		foreach (var key in provider.GetChildKeys(Enumerable.Empty<string>(), rootKey))
		{
			string surrogateKey = key;
			if (rootKey != null)
			{
				surrogateKey = rootKey + ":" + key;
			}

			GetFullKeyNames(provider, surrogateKey, initialKeys);

			if (!initialKeys.Any(k => k.StartsWith(surrogateKey)))
			{
				initialKeys.Add(surrogateKey);
			}
		}

		return initialKeys;
	}
}
