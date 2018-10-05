using System.Collections.Generic;

namespace V8Media.Framework.Models.Custom
{
    public class GoogleMaps
    {
        public IEnumerable<GoogleMapsLocation> Locations { get; set; }
        public string Id { get; set; }

        public GoogleMaps()
        {
            Locations = new List<GoogleMapsLocation>();
        }
    }
}