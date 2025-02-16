using SensorsREST.Models;

namespace SensorsREST.Utils;

public static class SensorDataHelper
{
    public static List<Candle> calculateMinuteCandles(List<SensorDatum> sensorData)
    {
        List<Candle> resultCandles = new List<Candle>();
        var groupedSensorData = sensorData.GroupBy(x => x.Timestamp.AddSeconds(-x.Timestamp.Second));
        foreach(var group in groupedSensorData)
        {
            resultCandles.Add(new Candle()
            {
                Close = group.OrderByDescending(x => x.Timestamp).First().Value,
                Open = group.OrderBy(x => x.Timestamp).First().Value,
                High = group.Max(x => x.Value),
                Low = group.Min(x => x.Value),
                StartOfCandle = group.Key.AddSeconds(-group.Key.Second)
            });
        }

        return resultCandles;
    }
}