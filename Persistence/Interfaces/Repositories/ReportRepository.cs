using Domain.Domain;
using Domain.Domain.Abstractions;
using Domain.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Interfaces.Repositories;
public class ReportRepository(IDatabaseContext _context) : IReportRepository
{
    public async Task AddReportAsync(Report report, CancellationToken cancellationToken)
    {
        await _context.Reports.AddAsync(report, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task ChangeStatusAsync(Guid reportId, ReportStatus status, CancellationToken cancellationToken)
    {
        await _context.Reports
            .Where(r => r.Id == reportId)
            .ExecuteUpdateAsync(call => call.SetProperty(r => r.ReportStatus, status), cancellationToken);
    }

    public async Task<IEnumerable<Report>> GetUnprocessedRepositoryAsync(CancellationToken cancellationToken) 
    {
        return await _context.Reports.Where(r => r.ReportStatus == Domain.Domain.Enums.ReportStatus.New).ToListAsync(cancellationToken);
    }
}