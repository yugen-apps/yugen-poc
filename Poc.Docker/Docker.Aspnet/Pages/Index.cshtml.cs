using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace Docker.Aspnet.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly MyDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public string ConnectionString => _configuration?.GetConnectionString("SQL") ?? string.Empty;
    public string CanConnect => _dbContext.Database.CanConnect().ToString();

    public string Txt { get; set; }

    public IndexModel(
        ILogger<IndexModel> logger,
        MyDbContext dbContext,
        IConfiguration configuration)
    {
        _logger = logger;
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public void TestRead()
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

    public void TestWrite()
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
