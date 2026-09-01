using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Poc.Common;

public readonly struct EnvironmentInfo
{
	private const double Mebi = 1024 * 1024;
	private const double Gibi = Mebi * 1024;

	public EnvironmentInfo()
	{
		GCMemoryInfo gcInfo = GC.GetGCMemoryInfo();
		TotalAvailableMemoryBytes = gcInfo.TotalAvailableMemoryBytes;

		if (!OperatingSystem.IsLinux())
		{
			return;
		}

		string[] memoryLimitPaths = new string[]
		{
			"/sys/fs/cgroup/memory.max",
			"/sys/fs/cgroup/memory.high",
			"/sys/fs/cgroup/memory.low",
			"/sys/fs/cgroup/memory/memory.limit_in_bytes",
		};

		string[] currentMemoryPaths = new string[]
		{
			"/sys/fs/cgroup/memory.current",
			"/sys/fs/cgroup/memory/memory.usage_in_bytes",
		};

		MemoryLimit = GetBestValue(memoryLimitPaths);
		MemoryUsage = GetBestValue(currentMemoryPaths);
	}

	public string RuntimeVersion => RuntimeInformation.FrameworkDescription;

	public string OSVersion => RuntimeInformation.OSDescription;

	public string CpuArchitecture => RuntimeInformation.OSArchitecture.ToString();

	public int CpuCores => Environment.ProcessorCount;

	public bool Containerized => Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") is not null;

	public string UserName => Environment.UserName;

	public long MemoryLimit { get; }

	// cgroup memory limit
	public string MemoryLimitString => GetInBestUnit(MemoryLimit);

	public long MemoryUsage { get; }

	//	cgroup memory usage
	public string MemoryUsageString => GetInBestUnit(MemoryUsage);

	public long TotalAvailableMemoryBytes { get; }

	// Memory, total available GC memory
	public string TotalAvailableMemoryBytesString => GetInBestUnit(TotalAvailableMemoryBytes);

	public string HostName => Dns.GetHostName();

	public async Task<IPAddress[]> IpList() =>
		await Dns.GetHostAddressesAsync(HostName);

	public async Task<string> IpListString() =>
		string.Join(", ", await IpList());

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

	private static string GetInBestUnit(long size)
	{
		if (size == 0)
		{
			return "0";
		}
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
}
