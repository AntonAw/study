using Domain.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Persistence.Interfaces;
public interface IDatabaseContext
{
    DatabaseFacade Database { get; }
    DbSet<Report> Reports { get; set; }
    DbSet<ReportForm> Forms { get; set; }
    DbSet<Theme> Themes { get; set; }
    DbSet<User> Users { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}