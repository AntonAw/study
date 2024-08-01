using Infrastructure.SendNewReports.Refit.Request;
using Infrastructure.SendNewReports.Refit.Response;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SendNewReports.Refit;
public interface IReportSenderRefit
{
    [Post("/url_to_microservice")]
    Task<ReportSendResponse> Send(ReportSendRequest request, CancellationToken cancellationToken);
}