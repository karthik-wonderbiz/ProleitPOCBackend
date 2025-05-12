using Microsoft.AspNetCore.Mvc;
using ProleitPocBackend.Model;
using ProleitPOCBackend.IService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProleitPocBackend.API.Controllers
{
    [ApiVersion("1.0")]
    //[ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _service;

        public DeviceController(IDeviceService service)
        {
            _service = service;
        }

        //[HttpGet("version")]
        //[MapToApiVersion("1.0")]
        //public IActionResult GetV1() => Ok("API v1.0 Response");

        //[HttpGet("version")]
        //[MapToApiVersion("2.0")]
        //public IActionResult GetV2() => Ok("API v2.0 Response");

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

        [HttpGet("machines-properties")]
        public async Task<IActionResult> GetMachinesAndProperties()
        {
            var machines = await _service.GetMachinesAsync();
            var properties = await _service.GetPropertiesAsync();

            var result = new
            {
                Machines = machines,
                Properties = properties
            };

            return Ok(result);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredData([FromQuery] DataFilter filter)
        {
            var result = await _service.GetFilteredDataAsync(filter);
            return Ok(result);
        }

        [HttpGet("aggregate-values")]
        public async Task<IActionResult> GetAggregateValues(
            [FromQuery] string machine,
            [FromQuery] string property,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            if (string.IsNullOrEmpty(machine) || string.IsNullOrEmpty(property))
            {
                return BadRequest("Machine and Property are required.");
            }

            var result = await _service.GetAggregateValuesAsync(machine, property, startDate, endDate);

            return Ok(result);
        }

    }
}
