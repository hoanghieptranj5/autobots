using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories.Models.ElectricCalculator;
using Repositories.Models.HanziCollector;

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
  public virtual DbSet<ElectricPrice> ElectricPrices { get; set; }
  public virtual DbSet<Hanzi> Hanzis { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);

    if (!optionsBuilder.IsConfigured)
    {
    }
  }
}