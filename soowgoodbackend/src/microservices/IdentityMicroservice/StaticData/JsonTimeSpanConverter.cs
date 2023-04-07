using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace IdentityMicroservice.StaticData
{
    public class JsonTimeSpanConverter : JsonConverter<TimeSpan>
    {
        public const string TimeSpanFormatString = "hh:mm tt";
        public override void WriteJson(JsonWriter writer, TimeSpan value, Newtonsoft.Json.JsonSerializer serializer)
        {

            DateTime time = DateTime.Today.Add(value);
            string timespanFormatted = time.ToString(TimeSpanFormatString, CultureInfo.InvariantCulture);
            writer.WriteValue(timespanFormatted);
        }

        public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            TimeSpan parsedTimeSpan;
            TimeSpan.TryParseExact((string)reader.Value, TimeSpanFormatString, null, out parsedTimeSpan);
            return parsedTimeSpan;
        }
    }
}
