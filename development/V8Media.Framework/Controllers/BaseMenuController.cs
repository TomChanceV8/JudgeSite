using System;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Models;
using V8Media.Framework.Domain;
using V8Media.Framework.Models.PublishedContent;
using V8Media.Framework.Mvc;
using V8Media.Framework.Utilities.Extensions;

namespace V8Media.Framework.Controllers
{
    /// <summary>
    /// Provides base Actions & methods for pages using/inherit from the Menu Doctype
    /// </summary>
    public abstract class BaseMenuController : SurfaceRenderMvcController
    {
        public virtual ActionResult RssFeed(RenderModel model)
        {
            if (model.Content is Menu)
            {
                return new RssResult(GetFeed(model.Content as Menu));
            }
            return HttpNotFound();
        }

        protected virtual SyndicationFeed GetFeed(Menu rootMenuModel)
        {
            var feed = new SyndicationFeed(
               rootMenuModel.RssTitle,
               rootMenuModel.RssFeedDescription,
               new Uri(rootMenuModel.UrlWithDomain()),
               GetFeedItems(rootMenuModel))
            {
                Generator = "V8Media Framework for Umbraco",
                //ImageUrl = ?
            };

            //TODO: attempting to add media:thumbnail...
            //feed.AttributeExtensions.Add(new XmlQualifiedName("media", "http://www.w3.org/2000/xmlns/"), "http://search.yahoo.com/mrss/");

            return feed;
        }

        protected virtual IEnumerable<SyndicationItem> GetFeedItems(Menu rootMenuModel)
        {
            var rootUrl = rootMenuModel.Website().UrlWithDomain();
            var result = new List<SyndicationItem>();
            var childItems = rootMenuModel.DocumentTypeAlias == DocTypeAliases.Home ? rootMenuModel.Website().Children<Menu>() : rootMenuModel.Children<Menu>();

            foreach (var item in childItems)
            {
                var content = item.RssItemDescription.UmbracoMediaHrefToAbsoluteUri(rootUrl);
                content = content.UmbracoMediaHrefToAbsoluteUri(rootUrl);

                var syndItem = new SyndicationItem(
                    item.RssTitle,
                    new TextSyndicationContent(content, TextSyndicationContentKind.Html),
                    new Uri(item.UrlWithDomain()),
                    item.Id.ToString(CultureInfo.InvariantCulture),
                    item.RssPubDate)
                {
                    PublishDate = item.RssPubDate,

                    //don't include this as it will override the main content bits
                    //Summary = new TextSyndicationContent(post.Excerpt)
                };

                //TODO: attempting to add media:thumbnail...
                //item.ElementExtensions.Add(new SyndicationElementExtension("thumbnail", "http://search.yahoo.com/mrss/", "This is a test!"));
                if (item is NewsArticle)
                {
                    var newsArticale = item as NewsArticle;
                    foreach (var c in newsArticale.CategoryTags)
                    {
                        syndItem.Categories.Add(new SyndicationCategory(c.Text));
                    }
                }
                


                result.Add(syndItem);
            }
            return result;
        }
    }
}