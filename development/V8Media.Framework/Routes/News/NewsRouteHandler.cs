using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace V8Media.Framework.Routes.News
{
    public class NewsRouteHandler : UmbracoVirtualNodeByIdRouteHandler
    {

       private struct UrlNames
        {
            public int NodeId { get; set; }
            public string UrlName { get; set; }
            public string PageName { get; set; }
        }

        private readonly List<UrlNames> _urlNames = new List<UrlNames>();

        public NewsRouteHandler(IEnumerable<IPublishedContent> itemsForRoute, string urlName, string pageName)
            : base(itemsForRoute)
        {
            foreach (var node in itemsForRoute)
            {
                _urlNames.Add(new UrlNames
                {
                    NodeId = node.Id,
                    UrlName = urlName,
                    PageName = pageName
                });
            }
        }

        public NewsRouteHandler(int realNodeId,
            string urlName,
            string pageName)
            : base(realNodeId)
        {
            _urlNames.Add(new UrlNames
            {
                NodeId = realNodeId,
                PageName = pageName,
                UrlName = urlName
            });
        }

        protected override IPublishedContent FindContent(RequestContext requestContext, UmbracoContext umbracoContext, IPublishedContent baseContent)
        {
            var urlNames = _urlNames.Single(x => x.NodeId == baseContent.Id);

            var controllerName = requestContext.RouteData.GetRequiredString("controller");
            var rootUrl = baseContent.Url;

            return new VirtualPage(
                baseContent,
                urlNames.PageName,
                controllerName,
                rootUrl.EnsureEndsWith('/') + urlNames.UrlName);
        }

    }
}