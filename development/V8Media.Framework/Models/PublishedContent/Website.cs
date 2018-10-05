using System.Collections.Generic;
using Umbraco.Web;
using V8Media.Framework.Domain;
using V8Media.Framework.Utilities.Extensions;
using Zbu.ModelsBuilder;

namespace V8Media.Framework.Models.PublishedContent
{
    [RenamePropertyType(PropertyAliases.UmbracoInternalRedirectId, "InternalUrlRedirect")]
    public partial class Website
    {
        [ImplementPropertyType(PropertyAliases.WebsiteSitemapPriority)]
        public string SitemapPriority
        {
            get { return this.GetPropertyValue<string>(PropertyAliases.WebsiteSitemapPriority); }

        }

        [ImplementPropertyType(PropertyAliases.WebsiteSitemapChangeFreq)]
        public string SitemapFrequency
        {
            get { return this.GetPropertyValue<string>(PropertyAliases.WebsiteSitemapChangeFreq); }
        }

        [ImplementPropertyType(PropertyAliases.WebsiteGoogleAnalytics)]
        public string GoogleAnalytics
        {
            get { return this.GetPropertyValue<string>(PropertyAliases.WebsiteGoogleAnalytics); }
        }

        [ImplementPropertyType(PropertyAliases.WebsiteHeaderLinks)]
        public IEnumerable<UrlPicker.Umbraco.Models.UrlPicker> HeaderLinks
        {
            get { return this.MultiLUrlPicker(PropertyAliases.WebsiteHeaderLinks); }
        }

        [ImplementPropertyType(PropertyAliases.WebsiteFooterLinks)]
        public IEnumerable<UrlPicker.Umbraco.Models.UrlPicker> FooterLinks
        {
            get { return this.MultiLUrlPicker(PropertyAliases.WebsiteFooterLinks); }
        }
    }
}