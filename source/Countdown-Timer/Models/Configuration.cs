using Microsoft.Extensions.Configuration;

namespace Countdown_Timer.Models
{
    public sealed class Configuration : Abstractions.IConfiguration
    {
        private static Configuration _instance;
        //---------------------------------------------------------------------
        internal static Configuration Default => _instance ?? (_instance = CreateConfiguration());
        //---------------------------------------------------------------------
        private static Configuration CreateConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("Countdown-Timer.config.json", optional: true)
                .AddJsonFile("Countdown-Timer.config.Debug.json", optional: true)
                .Build();

            var section = config.GetSection("Settings");
            return section.Get<Configuration>();
        }
        //---------------------------------------------------------------------
        public int StartSeconds     { get; set; } = 30;
        public int SecondsForYellow { get; set; } = 5;
        public int StartBeeping     { get; set; } = 5;
        public int FontSize         { get; set; } = 640;
    }
}
