using System;
using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Core.Models;
using V8Media.Framework.Domain;
using V8Media.Framework.Models.Custom;
using V8Media.Framework.Services;
using V8Media.Framework.Utilities.Extensions;
using Zbu.ModelsBuilder;

namespace V8Media.Framework.Models.PublishedContent
{
    [RenamePropertyType(PropertyAliases.NewsListingHeader, "Header")]
    public partial class NewsListing
    {
        public NewsListing(IPublishedContent content)
            : base(content)
        {
            NewsArticles = new List<PublishedContent.NewsArticle>();
        }

        #region Non DocType Properties

        /// <summary>
        /// List of child news items dependent on TagFilter & DateFilter
        /// </summary>
        /// <value>
        /// The news articles.
        /// </value>
        public IEnumerable<PublishedContent.NewsArticle> NewsArticles { get; set; }

        public Pager Pager { get; set; }

        /// <summary>
        /// List of archive date
        /// </summary>
        private IEnumerable<ArchiveDate> _newsArchiveDates;
        public IEnumerable<ArchiveDate> NewsArchiveDates
        {
            get { return _newsArchiveDates ?? (_newsArchiveDates = ServiceFactory.GetService<NewsService>().GetArchiveDates(this)); }
        }

        /// <summary>
        /// Gets or sets the current filtered archive year.
        /// </summary>
        /// <value>
        /// The filtered archive date.
        /// </value>
        public DateTime? FilteredArchiveYear { get; set; }

        /// <summary>
        /// Gets or sets the current filtered archive month.
        /// </summary>
        /// <value>
        /// The filtered archive month.
        /// </value>
        public DateTime? FilteredArchiveMonth { get; set; }

        /// <summary>
        /// Gets or sets the current filtered tag.
        /// </summary>
        /// <value>
        /// The filtered tag.
        /// </value>
        public string FilteredTag { get; set; }

        #region Urls

        /// <summary>
        /// Creates the tag filter url for categories
        /// </summary>
        /// <param name="categoryTag">The category tag.</param>
        /// <returns></returns>
        public string TagFilterUrl(string categoryTag)
        {
            return string.Format("{0}{1}/{2}", this.IsVirtualPage() ? Parent.Url.EnsureEndsWith('/') : Url.EnsureEndsWith('/'), TagUrlName, categoryTag);
        }

        /// <summary>
        /// Creates the tag filter url for a given tag group
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="group">The group.</param>
        /// <returns></returns>
        public string TagFilterUrl(string tag, string group)
        {
            return string.Format("{0}{1}/{2}/{3}", this.IsVirtualPage() ? Parent.Url.EnsureEndsWith('/') : Url.EnsureEndsWith('/'), TagUrlName, tag, group);
        }

        /// <summary>
        /// Creates the archive url for a given year
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns></returns>
        public string ArchiveUrl(string year)
        {
            return string.Format("{0}{1}/{2}", this.IsVirtualPage() ? Parent.Url.EnsureEndsWith('/') : Url.EnsureEndsWith('/'), ArchiveUrlName, year);
        }

        /// <summary>
        /// Creates the archive url for a given year and month
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <returns></returns>
        public string ArchiveUrl(string year, string month)
        {
            return string.Format("{0}{1}/{2}/{3}", this.IsVirtualPage() ? Parent.Url.EnsureEndsWith('/') : Url.EnsureEndsWith('/'), ArchiveUrlName, year, month);
        }

        #endregion

        #endregion
    }
}