using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using V8Media.Framework.Models.Custom;
using V8Media.Framework.Utilities.Extensions;

namespace V8Media.Framework.Utilities.PropertyValueConverters.uMap
{
    public class UMapValueConverter : PropertyValueConverterBase, IPropertyValueConverterMeta
    {
        //public override object ConvertSourceToObject(PublishedPropertyType propertyType, object source, bool preview)
        //{
        //    var googleMaps = new GoogleMaps();

        //    if (source == null) return googleMaps;

        //    var sourceString = source.ToString();

        //    if (!sourceString.DetectIsJson()) return googleMaps;

        //    try
        //    {
        //        var locations = JsonConvert.DeserializeObject<IEnumerable<GoogleMapsLocation>>(sourceString);
        //        googleMaps.Locations = locations;
        //    }
        //    catch (Exception)
        //    {
        //        return googleMaps;
        //    }
        //    return googleMaps;
        //}

        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            return "V8.uMap".InvariantEquals(propertyType.PropertyEditorAlias);
        }

        public Type GetPropertyValueType(PublishedPropertyType propertyType)
        {
            return typeof (GoogleMaps);
        }

        public PropertyCacheLevel GetPropertyCacheLevel(PublishedPropertyType propertyType, PropertyCacheValue cacheValue)
        {
            return PropertyCacheLevel.Content;
        }
    }
}