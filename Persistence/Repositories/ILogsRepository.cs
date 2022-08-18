using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public interface ILogsRepository
    {
        Task<List<ShiftEntityLog>> CreateNewLogsCommand(List<ShiftEntityLog> Logs);
        Task<List<ShiftEntityLog>> GetLogsQueries(List<Guid> logsIds);
    }
}
