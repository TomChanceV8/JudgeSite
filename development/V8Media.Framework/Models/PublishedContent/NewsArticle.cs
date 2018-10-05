using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web;
using Umbraco.Web.Models;
using V8Media.Framework.Domain;
using V8Media.Framework.Utilities.Extensions;
using Zbu.ModelsBuilder;

namespace V8Media.Framework.Models.PublishedContent
{
    [RenamePropertyType(PropertyAliases.NewsHeader, "Header")]
    public partial class NewsArticle
    {
        [ImplementPropertyType(PropertyAliases.NewsCategoriesTags)]
        public IEnumerable<TagModel> CategoryTags
        {
            // TODO Create a PropertyValueConverter for Tags Editor: https://github.com/umbraco/Umbraco4Docs/blob/master/Documentation/Extending-Umbraco/Property-Editors/value-converters-v7.md
            get { return Umbraco.TagQuery.GetTagsForProperty(Id, PropertyAliases.NewsCategoriesTags); }
            //get { return this.GetPropertyValue<string>(PropertyAliases.NewsCategoriesTags).Split(','); }
        }

        [ImplementPropertyType(PropertyAliases.V8MediaExcerpt)]
		public string Excerpt
		{
            get
            {
                return this.WillWork(PropertyAliases.V8MediaExcerpt)
                    ? this.GetPropertyValue<string>(PropertyAliases.V8MediaExcerpt)
                    : MetaDescription;
            }
		}


        #region Navigation Properties

        public PublishedContent.NewsListing NewsListing
        {
            get { return this.AncestorOrSelf<PublishedContent.NewsListing>(); }
        }

        #endregion
    }
}