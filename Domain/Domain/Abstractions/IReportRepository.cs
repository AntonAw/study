using Domain.Domain.Enums;

namespace Domain.Domain.Abstractions;
public interface IReportRepository
{
    Task<IEnumerable<Report>> GetUnprocessedRepositoryAsync(CancellationToken cancellationToken);
    Task AddReportAsync(Report report, CancellationToken cancellationToken);
    Task ChangeStatusAsync(Guid reportId, ReportStatus status, CancellationToken cancellationToken);
}