using ProleitPocBackend.Model;

namespace ProleitPocBackend.IRepository
{
    public interface IDeviceRepository
    {
        Task<IEnumerable<string>> GetMachinesAsync();
        Task<IEnumerable<string>> GetPropertiesAsync();
        Task<IEnumerable<Device>> GetFilteredDataAsync(DataFilter filter);
        Task<List<AggregateValue>> GetAggregateValuesAsync(string machine, string property, DateTime startDate, DateTime endDate);
    }
}
