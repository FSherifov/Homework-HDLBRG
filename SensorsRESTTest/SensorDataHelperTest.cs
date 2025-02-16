using SensorsREST.Models;
using SensorsREST.Utils;
using FluentAssertions;

namespace SensorsRESTTest;

public class SensorDataHelperTest
{
    [Fact]
    public void CalculateMinuteCandlesTest()
    {
        // Arrange
        var sensorData = new List<SensorDatum>
        {
            new SensorDatum { Timestamp = new DateTime(2025, 2, 15, 12, 0, 5), Value = 10 },
            new SensorDatum { Timestamp = new DateTime(2025, 2, 15, 12, 0, 30), Value = 15 },
            new SensorDatum { Timestamp = new DateTime(2025, 2, 15, 12, 0, 45), Value = 20 },
            new SensorDatum { Timestamp = new DateTime(2025, 2, 15, 12, 1, 10), Value = 25 },
            new SensorDatum { Timestamp = new DateTime(2025, 2, 15, 12, 1, 40), Value = 30 }
        };

        // Act
        var candles = SensorDataHelper.calculateMinuteCandles(sensorData);

        // Assert
        candles.Should().HaveCount(2); // Two different minute intervals

        var firstCandle = candles.First(c => c.StartOfCandle == new DateTime(2025, 2, 15, 12, 0, 0));
        firstCandle.Open.Should().Be(10);
        firstCandle.Close.Should().Be(20);
        firstCandle.High.Should().Be(20);
        firstCandle.Low.Should().Be(10);

        var secondCandle = candles.First(c => c.StartOfCandle == new DateTime(2025, 2, 15, 12, 1, 0));
        secondCandle.Open.Should().Be(25);
        secondCandle.Close.Should().Be(30);
        secondCandle.High.Should().Be(30);
        secondCandle.Low.Should().Be(25);
    }
}