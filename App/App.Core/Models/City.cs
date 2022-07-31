using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App.Core.Models
{
    public class City
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Content { get; set; }

        [JsonPropertyName("time_zone")]
        public string TimeZone { get; set; }

        public Country Country { get; set; }

        public override string ToString()
        {
            var str = new StringBuilder();
            str.AppendLine(new string('=', 60));
            str.AppendLine($"CITY INFO FOR {Name}");
            str.AppendLine(new string('=', 60));

            str.AppendLine($"Id: {Id}");
            str.AppendLine($"Name: {Name}");
            str.AppendLine($"Code: {Code}");
            str.AppendLine($"Time Zone: {TimeZone}");

            return str.ToString();
        }
    }
}
