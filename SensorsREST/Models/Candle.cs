namespace SensorsREST.Models;

public struct Candle
{
    public DateTime StartOfCandle{get; set;}
    public float High{get; set;}
    public float Low{get; set;}
    public float Open{get; set;}
    public float Close{get; set;}

}