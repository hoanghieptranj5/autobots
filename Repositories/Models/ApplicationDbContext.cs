using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Repositories.Models;

public partial class ApplicationDbContext : DbContext
{
    private readonly ILogger _logger;

    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILoggerFactory loggerFactory)
        : base(options)
    {
        _logger = loggerFactory.CreateLogger("DbLogger");
    }

    public virtual DbSet<Car> Cars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);

        if (!optionsBuilder.IsConfigured)
        {
            string connectionString = "Server=tcp:mysqlserverhip.database.windows.net,1433;Initial Catalog=autobot;Persist Security Info=False;User ID=azureuser;Password=Protoss5195;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}