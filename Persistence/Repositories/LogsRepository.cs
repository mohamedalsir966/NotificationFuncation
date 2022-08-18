using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class LogsRepository : ILogsRepository
    {
        private readonly ApplicationDbContext _context;
        public LogsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ShiftEntityLog>> CreateNewLogsCommand(List<ShiftEntityLog> logs)
        {
            _context.ShiftEntityLog.AddRange(logs);
            await _context.SaveChangesAsync();
            return logs;
        }
        public async Task<List<ShiftEntityLog>> GetLogsQueries(List<Guid> logsIds)
        {
            var ExistingList = await _context.ShiftEntityLog.Where(x => logsIds.Contains(x.Id)).ToListAsync();
            return ExistingList;
        }


    }
}
