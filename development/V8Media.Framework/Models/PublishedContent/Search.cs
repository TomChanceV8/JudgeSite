using System.Collections.Generic;
using Umbraco.Core.Models;
using V8Media.Framework.Domain;
using V8Media.Framework.Models.Custom;
using Zbu.ModelsBuilder;

namespace V8Media.Framework.Models.PublishedContent
{
    [RenamePropertyType(PropertyAliases.SearchHeader, "Header")]
    public partial class Search
    {
        public Search(IPublishedContent content)
            : base(content)
        {
            SearchResults = new List<IPublishedContent>();
        }

        #region Non DocType Properties

        /// <summary>
        /// Term used to search
        /// </summary>
        /// <value>
        /// The term.
        /// </value>
        public string Term { get; set; }

        /// <summary>
        /// List of search results from term.
        /// </summary>
        /// <value>
        /// The search results.
        /// </value>
        public IEnumerable<IPublishedContent> SearchResults { get; set; }

        public Pager Pager { get; set; }

        #endregion
    }
}