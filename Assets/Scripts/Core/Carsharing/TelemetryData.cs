using Newtonsoft.Json;
using System;

namespace Core.Carsharing
{
    public class TelemetryData
    {
        [JsonProperty("timedate")]
        public DateTime Timedate { get; set; }

        [JsonProperty("car_id")]
        public int CarId { get; set; }

        [JsonProperty("data")]
        public TelemetryDataDetails Data { get; set; }
    }
}