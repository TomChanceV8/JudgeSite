using System.Collections.Generic;
using System.Globalization;

namespace V8Media.Framework.Models.Custom
{
    public class GoogleMapsLocation
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Address { get; set; }
        public bool Disabled { get; set; }
        public IDictionary<string, string> Properties { get; set; }
        public string Location
        {
            get
            {
                return string.IsNullOrEmpty(Lat.ToString(CultureInfo.InvariantCulture)) ? Address : string.Concat(Lat, ",", Lng);
            }
        }

        public GoogleMapsLocation()
        {
            Properties = new Dictionary<string, string>();
        }
    }
}