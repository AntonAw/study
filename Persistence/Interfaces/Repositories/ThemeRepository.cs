using Domain.Domain;
using Domain.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Interfaces.Repositories;
public class ThemeRepository(IDatabaseContext _context) : IThemeRepository
{
    public async Task<IEnumerable<Theme>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Themes.Include(t => t.ReportForm).ToListAsync();
    }

    public async Task<Theme> GetByCommandAsync(string command, CancellationToken cancellationToken)
    {
        return await _context.Themes.Where(t => t.ThemeCommand == command).FirstOrDefaultAsync() 
            ?? throw new ArgumentNullException($"Theme with command {command} not found");
    }

    public async Task<ReportForm> GetReportFormByThemeCommandAsync(string command, CancellationToken cancellationToken)
    {
        var theme = await _context.Themes
            .Include(t => t.ReportForm)
            .FirstOrDefaultAsync(t => t.ThemeCommand == command)
            ?? throw new ArgumentNullException($"Theme with command {command} not found");
        return theme.ReportForm;
    }
}