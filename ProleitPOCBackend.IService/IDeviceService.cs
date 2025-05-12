using ProleitPocBackend.Model;

namespace ProleitPOCBackend.IService
{
    public interface IDeviceService
    {
        Task<IEnumerable<string>> GetMachinesAsync();
        Task<IEnumerable<string>> GetPropertiesAsync();
        Task<IEnumerable<Device>> GetFilteredDataAsync(DataFilter filter);
        Task<List<AggregateValue>> GetAggregateValuesAsync(string machine, string property, DateTime startDate, DateTime endDate);
    }
}
