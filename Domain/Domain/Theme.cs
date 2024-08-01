namespace Domain.Domain;
public class Theme
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ThemeCommand { get; set; }
    public string? Message { get; set; }
    public ReportForm? ReportForm { get; set; }
}