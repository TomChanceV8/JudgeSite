using System;
using System.Web;
using Umbraco.Web;
using V8Media.Framework.Domain;
using V8Media.Framework.Utilities.Extensions;
using Zbu.ModelsBuilder;

namespace V8Media.Framework.Models.PublishedContent
{
    [RenamePropertyType(PropertyAliases.UmbracoNaviHide, "HideInMenu")]
    public partial class Menu
    {
        [ImplementPropertyType(PropertyAliases.MenuMenuTitle)]
        public string MenuTitle
        {
            get { return Umbraco.Coalesce(this.GetPropertyValue<string>(PropertyAliases.MenuMenuTitle), Name); }
        }

        [ImplementPropertyType(PropertyAliases.MenuSitemapPriority)]
        public string SitemapPriority
        {
            get
            {
                if (!String.IsNullOrEmpty(this.GetPropertyValue<string>(PropertyAliases.MenuSitemapPriority)))
                {
                    return this.GetPropertyValue<string>(PropertyAliases.MenuSitemapPriority);
                }
                return this.Content.Website().SitemapPriority;
            }
        }

        [ImplementPropertyType(PropertyAliases.MenuSitemapChangeFreq)]
        public string SitemapFrequency
        {
            get
            {
                if (!String.IsNullOrEmpty(this.GetPropertyValue<string>(PropertyAliases.MenuSitemapChangeFreq)))
                {
                    return this.GetPropertyValue<string>(PropertyAliases.MenuSitemapChangeFreq);
                }
                return this.Content.Website().SitemapFrequency;
            }
        }

        #region Non DocType Properties

        public virtual string RssTitle 
        {
            get
            {
                return this.WillWork(PropertyAliases.V8MediaHeader)
                    ? this.GetPropertyValue<string>(PropertyAliases.V8MediaHeader)
                    : MenuTitle;
            }
        }

        public virtual string RssFeedDescription
        {
            get
            {
                return this.WillWork(PropertyAliases.V8MediaExcerpt)
                    ? this.GetPropertyValue<string>(PropertyAliases.V8MediaExcerpt)
                    : MetaDescription;
            }
        }

        public virtual string RssItemDescription
        {
            get
            {
                return this.WillWork(PropertyAliases.V8MediaExcerpt)
                    ? this.GetPropertyValue<string>(PropertyAliases.V8MediaExcerpt)
                    : this.WillWork(PropertyAliases.V8MediaBodyText) ? this.GetPropertyValue<HtmlString>(PropertyAliases.V8MediaBodyText).ToString() : MetaDescription;
            }
        }

        public virtual DateTime RssPubDate
        {
            get
            {
                return this.WillWork(PropertyAliases.NewsNewsDate)
                    ? this.GetPropertyValue<DateTime>(PropertyAliases.NewsNewsDate)
                    : CreateDate;
            }
        }

        #endregion
    }
}