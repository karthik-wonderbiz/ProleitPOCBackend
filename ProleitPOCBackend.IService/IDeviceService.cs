using ProleitPocBackend.Model;

namespace ProleitPOCBackend.IService
{
    public interface IDeviceService
    {
        Task<IEnumerable<string>> GetMachinesAsync();
        Task<IEnumerable<string>> GetPropertiesAsync();
        Task<IEnumerable<Device>> GetFilteredDataAsync(DataFilter filter);
        Task<List<DailyStatistic>> GetDailyStatisticsAsync(string machine, string property, DateTime startDate, DateTime endDate);
    }
}
