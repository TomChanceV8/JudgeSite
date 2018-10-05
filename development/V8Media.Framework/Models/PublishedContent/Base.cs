using System;
using Umbraco.Core.Models;
using Umbraco.Web;
using V8Media.Framework.Domain;
using V8Media.Framework.Utilities.Extensions;
using Zbu.ModelsBuilder;

namespace V8Media.Framework.Models.PublishedContent
{
    public partial class Base
    {
        protected UmbracoHelper Umbraco
        {
            get { return new UmbracoHelper(UmbracoContext.Current); }
        }

        #region MetaData

        [ImplementPropertyType(PropertyAliases.BasePageTitle)]
        public string PageTitle
        {
            get
            {
                if (!String.IsNullOrEmpty(this.GetPropertyValue<string>(PropertyAliases.BasePageTitle)))
                {
                    return this.GetPropertyValue<string>(PropertyAliases.BasePageTitle);
                }
                return Name + " " + this.Website().SiteName;
            }
        }

        [ImplementPropertyType(PropertyAliases.BaseMetaDescription)]
        public string MetaDescription
        {
            get { return this.GetPropertyValue<string>(PropertyAliases.BaseMetaDescription); }
        }

        [ImplementPropertyType(PropertyAliases.BaseMetaKeywords)]
        public string MetaKeywords
        {
            get { return this.GetPropertyValue<string>(PropertyAliases.BaseMetaKeywords); }
        }

        #endregion

        #region OpenGraph

        [ImplementPropertyType(PropertyAliases.BaseOpenGraphTitle)]
        public string OpenGraphTitle
        {
            get
            {
                if (!String.IsNullOrEmpty(this.GetPropertyValue<string>(PropertyAliases.BaseOpenGraphTitle)))
                {
                    return this.GetPropertyValue<string>(PropertyAliases.BaseOpenGraphTitle);
                }
                return PageTitle;
            }
        }

        [ImplementPropertyType(PropertyAliases.BaseOpenGraphDescription)]
        public string OpenGraphDescription
        {
            get
            {
                if (!String.IsNullOrEmpty(this.GetPropertyValue<string>(PropertyAliases.BaseOpenGraphDescription)))
                {
                    return this.GetPropertyValue<string>(PropertyAliases.BaseOpenGraphDescription);
                }
                return MetaDescription;
            }
        }

        [ImplementPropertyType(PropertyAliases.BaseOpenGraphImage)]
        public IPublishedContent OpenGraphImage
        {
            get
            {
                if (this.GetPropertyValue<IPublishedContent>(PropertyAliases.BaseOpenGraphImage) != null)
                {
                    return this.GetPropertyValue<IPublishedContent>(PropertyAliases.BaseOpenGraphImage);
                }
                return this.Website().OpenGraphImage;
            }
        }

        #endregion

        #region TwitterCard

        [ImplementPropertyType(PropertyAliases.BaseTwitterCardTitle)]
        public string TwitterCardTitle
        {
            get
            {
                if (!String.IsNullOrEmpty(this.GetPropertyValue<string>(PropertyAliases.BaseTwitterCardTitle)))
                {
                    return this.GetPropertyValue<string>(PropertyAliases.BaseTwitterCardTitle);
                }
                return PageTitle;
            }
        }

        [ImplementPropertyType(PropertyAliases.BaseTwitterCardDescription)]
        public string TwitterCardDescription
        {
            get
            {
                if (!String.IsNullOrEmpty(this.GetPropertyValue<string>(PropertyAliases.BaseTwitterCardDescription)))
                {
                    return this.GetPropertyValue<string>(PropertyAliases.BaseTwitterCardDescription);
                }
                return MetaDescription;
            }
        }

        [ImplementPropertyType(PropertyAliases.BaseTwitterCardImage)]
        public IPublishedContent TwitterCardImage
        {
            get
            {
                if (this.GetPropertyValue<IPublishedContent>(PropertyAliases.BaseTwitterCardImage) != null)
                {
                    return this.GetPropertyValue<IPublishedContent>(PropertyAliases.BaseTwitterCardImage);
                }
                return this.Website().TwitterCardImage;
            }
        }

        #endregion
    }
}