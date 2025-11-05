using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlertDengueApi.Models;

namespace AlertDengueApi.Interfaces
{
    public interface IDengueAlertService
    {
        Task<IEnumerable<DengueAlert>> FetchAndSaveLastSixMonthsAsync();
        Task<DengueAlert?> GetAlertByWeekAsync(int week, int year);
        Task<IEnumerable<DengueAlert>> GetAllAlertsAsync();
    }
}