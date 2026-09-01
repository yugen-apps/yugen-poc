using Microsoft.EntityFrameworkCore;

namespace Poc.Docker.Aspnet.Data;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }
}
