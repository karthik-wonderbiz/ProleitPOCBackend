using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProleitPocBackend.Data;
using ProleitPocBackend.Hubs;
using ProleitPocBackend.IRepository;
using ProleitPocBackend.Model;

namespace ProleitPocBackend.Repository
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly IHubContext<ProleitPocBackendHubs> _hubContext;
        private readonly ProleitPocBackendDbContext _dbContext;
        private readonly string _connectionString;
        private static bool _isSubscribed = false;  
        private static bool _isSubscribedToEvent = false;
        public DeviceRepository(IConfiguration configuration, IHubContext<ProleitPocBackendHubs> hubContext, ProleitPocBackendDbContext dbContext)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
            _hubContext = hubContext;
            _dbContext = dbContext;
            SqlDependency.Start(_connectionString);
            if (!_isSubscribed)
            {
                SubscribeToDeviceChanges();
                _isSubscribed = true; 
            }
        }
        public async Task<IEnumerable<string>> GetMachinesAsync()
        {
            return await _dbContext.devices.Select(m => m.Machine).Distinct().ToListAsync();
        }
        public async Task<IEnumerable<string>> GetPropertiesAsync()
        {
            return await _dbContext.devices.Select(m => m.Property).Distinct().ToListAsync();
        }
        public async Task<IEnumerable<Device>> GetFilteredDataAsync(DataFilter filter)
        {
            var query = _dbContext.devices.AsQueryable();
            if (!string.IsNullOrEmpty(filter.Machine)) query = query.Where(m => m.Machine == filter.Machine);
            if (!string.IsNullOrEmpty(filter.Property)) query = query.Where(m => m.Property == filter.Property);
            if (filter.MinValue.HasValue) query = query.Where(m => m.Value >= filter.MinValue.Value);
            if (filter.MaxValue.HasValue) query = query.Where(m => m.Value <= filter.MaxValue.Value);
            if (filter.StartDate.HasValue) query = query.Where(m => m.Timestamp >= filter.StartDate.Value);
            if (filter.EndDate.HasValue) query = query.Where(m => m.Timestamp <= filter.EndDate.Value);
            return await query.ToListAsync();
        }
        public async Task<List<AggregateValue>> GetAggregateValuesAsync(string machine, string property, DateTime startDate, DateTime endDate)
        {
            var result = await _dbContext.devices
                .Where(d => d.Machine == machine && d.Property == property && d.Timestamp >= startDate && d.Timestamp <= endDate)
                .GroupBy(d => d.Timestamp.Date) 
                .Select(g => new AggregateValue
                {
                    Date = g.Key,
                    MinValue = g.Min(d => d.Value),
                    MaxValue = g.Max(d => d.Value),
                    AvgValue = g.Average(d => d.Value)
                })
                .OrderBy(d => d.Date)
                .ToListAsync();
            return result;
        }
        private void SubscribeToDeviceChanges()
        {
            if (!_isSubscribedToEvent)
            {
                Console.WriteLine($"[SignalR] Subscribing to device changes at {DateTime.Now}");
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string commandText = "SELECT Id, Machine, Property, Value, Timestamp FROM dbo.Devices";
                    using (SqlCommand cmd = new SqlCommand(commandText, conn))
                    {
                        SqlDependency dependency = new SqlDependency(cmd);
                        dependency.OnChange += OnDeviceChange;
                        cmd.ExecuteReader();
                    }
                }
                _isSubscribedToEvent = true;
            }
            else
            {
                Console.WriteLine($"[SignalR] Already subscribed to device changes at {DateTime.Now}");
            }
        }
        private async void OnDeviceChange(object sender, SqlNotificationEventArgs e)
        {
            Console.WriteLine($"[SignalR] Database change detected at {DateTime.Now}");
            if (e.Type == SqlNotificationType.Change)
            {
                var latestDevice = GetLatestDevice();
                if (latestDevice != null)
                {
                    await _hubContext.Clients.All.SendAsync("refreshDevices", latestDevice);
                }
                _isSubscribedToEvent = false;
                SubscribeToDeviceChanges();
            }
        }
        private List<Device> GetLatestDevice(int seconds = 5)
        {
            List<Device> devices = new List<Device>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT Id, Machine, Property, Value, Timestamp
                    FROM dbo.Devices
                    WHERE Timestamp >= DATEADD(SECOND, -@Seconds, GETDATE())
                    ORDER BY Timestamp DESC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Seconds", seconds);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            devices.Add(new Device
                            {
                                Id = Guid.Parse(reader["Id"].ToString()!),
                                Machine = reader["Machine"].ToString()!,
                                Property = reader["Property"].ToString()!,
                                Value = Convert.ToInt32(reader["Value"]),
                                Timestamp = Convert.ToDateTime(reader["Timestamp"])
                            });
                        }
                    }
                }
            }
            return devices;
        }
        ~DeviceRepository()
        {
            SqlDependency.Stop(_connectionString);
        }
    }
}