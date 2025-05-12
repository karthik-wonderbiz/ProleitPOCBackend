using ProleitPocBackend.IRepository;
using ProleitPocBackend.Model;
using ProleitPOCBackend.IService;

namespace ProleitPOCBackend.Service
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _repository;

        public DeviceService(IDeviceRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<string>> GetMachinesAsync()
        {
            return await _repository.GetMachinesAsync();
        }

        public async Task<IEnumerable<string>> GetPropertiesAsync()
        {
            return await _repository.GetPropertiesAsync();
        }

        public async Task<IEnumerable<Device>> GetFilteredDataAsync(DataFilter filter)
        {
            return await _repository.GetFilteredDataAsync(filter);
        }

        public async Task<List<AggregateValue>> GetAggregateValuesAsync(string machine, string property, DateTime startDate, DateTime endDate)
        {
            return await _repository.GetAggregateValuesAsync(machine, property, startDate, endDate);
        }

    }
}
