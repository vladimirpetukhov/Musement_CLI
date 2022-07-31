using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models
{
    public class Weather
    {
        public Location Location { get; set; }
    }

    public class Location
    {
        public string Name { get; set; }

        public string Country { get; set; }

        public string LocalTime { get; set; }
    }

    public class Forecast
    {

    }

    public enum ForecastDay
    {

    }


}
