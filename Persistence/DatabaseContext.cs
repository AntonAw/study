using Domain.Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Interfaces;

namespace Persistence;
public class DatabaseContext : DbContext, IDatabaseContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<Theme> Themes { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<ReportForm> Forms { get; set; }
    public DbSet<User> Users { get; set; }
}