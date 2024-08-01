namespace Domain.Domain.Abstractions;
public interface IThemeRepository
{
    Task<IEnumerable<Theme>> GetAllAsync(CancellationToken cancellationToken);
    Task<Theme> GetByCommandAsync(string command, CancellationToken cancellationToken);
    Task<ReportForm> GetReportFormByThemeCommandAsync(string command, CancellationToken cancellationToken);
}