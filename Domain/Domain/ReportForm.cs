using Domain.Domain.Enums;

namespace Domain.Domain;
public class ReportForm
{
    public Guid Id { get; set; }
    public string Form { get; set; }
    public FormAction Action { get; set; }
}