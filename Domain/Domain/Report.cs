using Domain.Domain.Enums;

namespace Domain.Domain;
public class Report
{
    public Guid Id { get; set; }
    public Theme ReportTheme { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public string Message { get; set; }
    public ReportStatus ReportStatus { get; set; } = ReportStatus.New;
    public User User { get; set; }
}