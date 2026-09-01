using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Text.Json;

namespace Poc.Common.Web;

public class WebHostInfoService
{
    private static JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web) { WriteIndented = true };

    private readonly IWebHostEnvironment? _webHostEnvironment;
    private readonly Dictionary<string, string> _list = [];

    public WebHostInfoService(IWebHostEnvironment? webHostEnvironment)
    {
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
            _list.Add(item.Name, item.Value.ToString());
        }

        return _list;
    }
}
