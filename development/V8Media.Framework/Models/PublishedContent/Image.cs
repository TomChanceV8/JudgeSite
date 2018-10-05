using Umbraco.Web;
using V8Media.Framework.Domain;
using V8Media.Framework.Utilities.Extensions;
using Zbu.ModelsBuilder;

namespace V8Media.Framework.Models.PublishedContent
{
    public partial class Image
    {
        [ImplementPropertyType(PropertyAliases.V8MediaAltTag)]
        public string AltTag
        {
            get
            {
                return this.WillWork(PropertyAliases.V8MediaAltTag) ? this.GetPropertyValue<string>(PropertyAliases.V8MediaAltTag) : Name;
            }
        }
    }
}