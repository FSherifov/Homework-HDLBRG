using System;
using System.Collections.Generic;

namespace SensorsREST.Models;

public partial class SensorDatum
{
    public int Id { get; set; }

    public string Topic { get; set; } = null!;

    public float Value { get; set; }

    public DateTime Timestamp { get; set; }
}
