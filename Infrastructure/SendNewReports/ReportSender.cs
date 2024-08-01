using Domain.Domain.Abstractions;
using Infrastructure.SendNewReports.Refit;
using Quartz;

namespace Infrastructure.SendNewReports;
public class ReportSender(IReportRepository _reportRepository, IReportSenderRefit _sender) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine($"Report sender job was started: {DateTime.Now}");

        var reports = await _reportRepository.GetUnprocessedRepositoryAsync(context.CancellationToken);

        for (int i = 0; i < (uint)reports.Count(); i++)
        {
            //await _sender.Send(new ReportSendRequest(reports.ElementAt(i)), context.CancellationToken);
            //await _reportRepository.ChangeStatusAsync(reports.ElementAt(i).Id, Domain.Domain.Enums.ReportStatus.Processing, context.CancellationToken);
        }

        Console.WriteLine($"Report sender job was stoped: {DateTime.Now} \nSended: {reports.Count()}");
    }
}