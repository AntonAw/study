using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Application.Services.Abstarctions;
public interface ISalaryHelpService
{
    Task HelpAsync(CallbackQuery query, string command, CancellationToken cancellationToken);
}