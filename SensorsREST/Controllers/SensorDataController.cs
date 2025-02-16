using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SensorsREST.Models;
using SensorsREST.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SensorsREST.Controller;

[ApiController]
public class SensorDataController : ControllerBase
{
    private readonly MqttContext _context;

    public SensorDataController(MqttContext context)
    {
        _context = context;
    }

    // Take all available sensor data
    [HttpGet]
    [Route("api/sensor_data")]
    public async Task<ActionResult<IEnumerable<SensorDatum>>> GetSensorsData([FromQuery] DateTime? startTime, [FromQuery] DateTime? endTime, [FromQuery] string sensor="")
    {
        IQueryable<SensorDatum> tempSensorsData = _context.SensorData;
        if(sensor != "")
        {
            tempSensorsData = tempSensorsData.Where(x => x.Topic == sensor);
        }
        if (startTime.HasValue)
        {
            tempSensorsData = tempSensorsData.Where(x => x.Timestamp > startTime);
        }
        if(endTime.HasValue)
        {
            tempSensorsData = tempSensorsData.Where(x => x.Timestamp < endTime);
        }
        var sensorsData = await tempSensorsData.ToListAsync();
        return Ok(sensorsData);
    }

    // Take all available sensor data
    [HttpGet]
    [Route("api/sensor_data/candles")]
    public async Task<ActionResult<IEnumerable<SensorDatum>>> GetSensorsDataCandles([FromQuery] DateTime? startTime, [FromQuery] DateTime? endTime, [FromQuery] string sensor="")
    {
        IQueryable<SensorDatum> tempSensorsData = _context.SensorData;
        if(sensor != "")
        {
            tempSensorsData = tempSensorsData.Where(x => x.Topic == sensor);
        }
        if (startTime.HasValue)
        {
            tempSensorsData = tempSensorsData.Where(x => x.Timestamp > startTime);
        }
        if(endTime.HasValue)
        {
            tempSensorsData = tempSensorsData.Where(x => x.Timestamp < endTime);
        }
        var sensorsData = await tempSensorsData.ToListAsync();
        var candlestickData = SensorDataHelper.calculateMinuteCandles(sensorsData);
        return Ok(candlestickData);
    }
}
