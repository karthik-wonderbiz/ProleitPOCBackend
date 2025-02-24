using ProleitPocBackend.Model;

namespace ProleitPocBackend.IRepository
{
    public interface IDeviceRepository
    {
        List<Device> GetAllDevices();
        Task<IEnumerable<string>> GetMachinesAsync();
        Task<IEnumerable<string>> GetPropertiesAsync();
        Task<IEnumerable<Device>> GetFilteredDataAsync(DataFilter filter);
        Task<List<DailyStatistic>> GetDailyStatisticsAsync(string machine, string property, DateTime startDate, DateTime endDate);
    }
}
