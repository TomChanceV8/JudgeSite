using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;
using V8Media.Framework.Domain;
using V8Media.Framework.Models.Custom;
using V8Media.Framework.Models.PublishedContent;

namespace V8Media.Framework.Services
{
    public class NewsService : UmbracoBase
    {
        /// <summary>
        /// Gets the latest news.
        /// </summary>
        /// <param name="rootNode">The rootNode can either be a NewsListing node or higher level node to include news from multiple NewListing nodes as it uses DescendantsOrSelf.</param>
        /// <returns></returns>
        public virtual IEnumerable<NewsArticle> GetLatestNews(IPublishedContent rootNode)
        {
            return
                (
                    from n in rootNode.DescendantsOrSelf<NewsArticle>()
                    where !n.HideInMenu
                    orderby n.NewsDate descending 
                    select n
                );
        }

        /// <summary>
        /// Gets news filtered by tag
        /// </summary>
        /// <param name="rootNode">The rootNode can either be a NewsListing node or higher level node to include news from multiple NewListing nodes as it uses DescendantsOrSelf.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="tagGroup">The tag group.</param>
        /// <returns></returns>
        public virtual IEnumerable<NewsArticle> GetFilteredNews(IPublishedContent rootNode, string tag, string tagGroup)
        {
            var taggedNews = Umbraco.TagQuery.GetContentByTag(tag, tagGroup).OfType<NewsArticle>();
            if (rootNode.DocumentTypeAlias == DocTypeAliases.NewsListing)
            {
                return taggedNews.Where(n => n.Path.Contains(rootNode.Id.ToString(CultureInfo.InvariantCulture)));
            }
            return taggedNews;

        }

        /// <summary>
        /// Gets news filtered by year
        /// </summary>
        /// <param name="rootNode">The root node.</param>
        /// <param name="year">The year.</param>
        /// <returns></returns>
        public virtual IEnumerable<NewsArticle> GetFilteredNews(IPublishedContent rootNode, DateTime year)
        {
            return GetLatestNews(rootNode).Where(n => n.NewsDate.Year == year.Year);
        }

        /// <summary>
        /// Gets news filtered by year and month
        /// </summary>
        /// <param name="rootNode">The root node.</param>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <returns></returns>
        public virtual IEnumerable<NewsArticle> GetFilteredNews(IPublishedContent rootNode, DateTime year, DateTime month)
        {
            return GetLatestNews(rootNode).Where(n => n.NewsDate.Year == year.Year && n.NewsDate.Month == month.Month);
        }

        /// <summary>
        /// Gets the news category tags.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<ITag> GetCategoryTags()
        {
            return Services.TagService.GetAllContentTags(AppConstants.TagGroupNewsCategories);
        }

        /// <summary>
        /// Gets the archive dates.
        /// </summary>
        /// <param name="rootNode">The rootNode can either be a NewsListing node or higher level node to include news from multiple NewListing nodes as it uses DescendantsOrSelf.</param>
        /// <returns></returns>
        public virtual IEnumerable<ArchiveDate> GetArchiveDates(IPublishedContent rootNode)
        {
            return
                (
                    from n in rootNode.DescendantsOrSelf<NewsArticle>()
                    where !n.HideInMenu
                    group n by new {month = n.NewsDate.Month, year = n.NewsDate.Year } into d
                    select new ArchiveDate(new DateTime(d.Key.year, d.Key.month, 1), d.Count())
                );
        }

    }
}