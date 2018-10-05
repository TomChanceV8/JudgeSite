using System;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using V8Media.Framework.Domain;
using V8Media.Framework.Utilities.Extensions;

namespace V8Media.Framework.Routes
{
    /// <summary>
    /// Used to create a fake dynamic umbraco page for rendering tag lists, tag pages and search results (any virtual route)
    /// </summary>
    internal class VirtualPage : PublishedContentWrapped
    {
        private readonly string _pageName;
        private readonly string _pageTypeAlias;
        private readonly string _urlPath;

        public VirtualPage(IPublishedContent rootForumPage, string pageName, string pageTypeAlias, string urlPath = null)
            : base(rootForumPage)
        {
            if (pageName == null) throw new ArgumentNullException("pageName");
            if (pageTypeAlias == null) throw new ArgumentNullException("pageTypeAlias");
            _pageName = pageName;
            _pageTypeAlias = pageTypeAlias;

            if (urlPath != null)
            {
                _urlPath = urlPath.SafeEncodeUrlSegments();
            }

        }

        public override string CreatorName
        {
            get { return AppConstants.VirtualPageCreatorName; }
        }

        public override string Url
        {
            get { return base.Url.EnsureEndsWith('/') + (_urlPath ?? UrlName); }
        }

        /// <summary>
        /// Returns the content that was used to create this virtual node - we'll assume this virtual node's parent is based on the real node that created it
        /// </summary>
        public override IPublishedContent Parent
        {
            get { return Content; }
        }

        public override string Name
        {
            get { return _pageName; }
        }

        public override string UrlName
        {
            get { return _pageName.ToLowerInvariant(); }
        }

        public override string DocumentTypeAlias
        {
            get { return _pageTypeAlias; }
        }

        public override int Level
        {
            get { return Content.Level + 1; }
        }
    }
}