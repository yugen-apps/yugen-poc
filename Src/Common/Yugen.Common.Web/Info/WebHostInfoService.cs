using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Text.Json;

namespace Yugen.Common.Web.Info;

public class WebHostInfoService
{
    private static JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web) { WriteIndented = true };

    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly Dictionary<string, string> _list = [];

    public WebHostInfoService(
        IConfiguration configuration,
        IWebHostEnvironment webHostEnvironment)
    {
        _configuration = configuration;
        _webHostEnvironment = webHostEnvironment;
    }

    public Dictionary<string, string> Initialize()
    {
        var webHostEnvironment = JsonSerializer.Serialize(_webHostEnvironment, _jsonSerializerOptions);
        using var jsonDocument = JsonDocument.Parse(webHostEnvironment);

        var list = jsonDocument
            .RootElement
            .EnumerateObject();

        foreach (var item in list)
        {
            _list.Add(item.Name, $"{item.Value}");
        }

        _list.Add("IsDevelopment", $"{_webHostEnvironment.IsDevelopment()}");
        _list.Add("Environment", $"{_configuration["ASPNETCORE_ENVIRONMENT"]}");

        return _list;
    }
}
