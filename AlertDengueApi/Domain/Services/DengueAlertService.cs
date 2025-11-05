using System.Net.Http.Json;
using System.Text.Json;
using AlertDengueApi.Helpers;
using AlertDengueApi.Interfaces;
using AlertDengueApi.Models;


namespace AlertDengueApi.Services
{
    public class DengueAlertService : IDengueAlertService
    {
        private readonly IEpidemologicalWeekHelper _weekHelper;
        private readonly IDengueAlertRepository _repository;
        private readonly HttpClient _httpClient;

        public const string ExternalApiUrl = "https://info.dengue.mat.br/api/alertcity";

        public DengueAlertService(
            IEpidemologicalWeekHelper weekHelper,
            IDengueAlertRepository repository,
            HttpClient httpClient)
        {
            _weekHelper = weekHelper;
            _repository = repository;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<DengueAlert>> FetchAndSaveLastSixMonthsAsync()
        {
            try
            {
                var (ewStart, ewEnd, eyStart, eyEnd) = _weekHelper.GetLastSixMonths();

                var queryParams = new Dictionary<string, string>()
                {
                    { "geocode", "3106200" },
                    { "disease", "dengue" },
                    { "format", "json" },
                    { "ew_start", ewStart.ToString() },
                    { "ew_end", ewEnd.ToString() },
                    { "ey_start", eyStart.ToString() },
                    { "ey_end", eyEnd.ToString() }
                };

                var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
                var requestUrl = $"{ExternalApiUrl}?{queryString}";

                var response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                var alerts = JsonSerializer.Deserialize<List<DengueAlert>>(json) ?? new List<DengueAlert>();

                foreach (var alert in alerts)
                {
                    var exists = await _repository.GetByWeekAsync(alert.EpidemiologicalWeek, alert.EpidemiologicalYear);
                    if (exists == null)
                        await _repository.SaveAsync(alert);
                }

                return alerts;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error while fetching and saving dengue alerts.", ex);
            }
        }

        public async Task<DengueAlert?> GetAlertByWeekAsync(int epidemiologicalWeek, int epidemiologicalYear)
        {
            return await _repository.GetByWeekAsync(epidemiologicalWeek, epidemiologicalYear);
        }

        public async Task<IEnumerable<DengueAlert>> GetAllAlertsAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}