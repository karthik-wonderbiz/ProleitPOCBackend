using Microsoft.AspNetCore.Mvc;
using ProleitPocBackend.IRepository;
using ProleitPocBackend.Model;
using ProleitPOCBackend.IService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProleitPocBackend.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _service;

        public DeviceController(IDeviceService service)
        {
            _service = service;
        }

        [HttpGet("machines")]
        public async Task<IActionResult> GetMachines()
        {
            var result = await _service.GetMachinesAsync();
            return Ok(result);
        }

        [HttpGet("properties")]
        public async Task<IActionResult> GetProperties()
        {
            var result = await _service.GetPropertiesAsync();
            return Ok(result);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredData([FromQuery] DataFilter filter)
        {
            var result = await _service.GetFilteredDataAsync(filter);
            return Ok(result);
        }

        [HttpGet("daily-statistics")]
        public async Task<IActionResult> GetDailyStatistics(
            [FromQuery] string machine,
            [FromQuery] string property,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            if (string.IsNullOrEmpty(machine) || string.IsNullOrEmpty(property))
            {
                return BadRequest("Machine and Property are required.");
            }

            var statistics = await _service.GetDailyStatisticsAsync(machine, property, startDate, endDate);

            return Ok(statistics);
        }

    }
}
