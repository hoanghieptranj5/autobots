﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories.Models.ElectricCalculator;
using Repositories.Models.HanziCollector;
using Repositories.Models.Users;

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

    public virtual DbSet<ElectricPrice> ElectricPrices { get; set; }
    public virtual DbSet<Hanzi> Hanzis { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Vocabulary.Vocabulary> Vocabularies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);

        if (!optionsBuilder.IsConfigured)
        {

        }
    }
}
