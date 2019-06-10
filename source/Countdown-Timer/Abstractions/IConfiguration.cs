namespace Countdown_Timer.Abstractions
{
    public interface IConfiguration
    {
        int StartSeconds     { get; }
        int SecondsForYellow { get; }
        int StartBeeping     { get; }
        int FontSize         { get; }
    }
}
