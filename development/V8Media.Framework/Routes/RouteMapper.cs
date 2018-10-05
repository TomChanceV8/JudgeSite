using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.PublishedCache;
using V8Media.Framework.Domain;
using V8Media.Framework.Routes.News;

namespace V8Media.Framework.Routes
{
    public static class RouteMapper
    {
        public const string NewsArchiveRouteName = "v8media_news_archive_{0}";
        public const string NewsTagRouteName = "v8media_news_tag_{0}";

        public static void MapRoutes(RouteCollection routes, ContextualPublishedCache umbracoCache)
        {
            var newsListingNodes = umbracoCache.GetByXPath(string.Concat("//", DocTypeAliases.NewsListing)).ToArray();
            //NOTE: need to write lock because this might need to be remapped while the app is running if
            // any V8MediaFramework virtual node holder nodes are updated with new values
            using (routes.GetWriteLock())
            {
                //clear the existing V8MediaFramework routes (if any)
                RemoveExisting(routes);

                MapNewsRoutes(routes, newsListingNodes);
            }
        }

        private static void MapNewsRoutes(RouteCollection routes, IEnumerable<IPublishedContent> newsListingNodes)
        {
            var groups = newsListingNodes.GroupBy(x => RouteCollectionExtensions.RoutePathFromNodeUrl(x.Url));
            foreach (var grouping in groups)
            {
                var nodesAsArray = grouping.ToArray();

                MapNewsArchiveRoute(routes, grouping.Key, nodesAsArray);
                MapNewsTagsRoute(routes, grouping.Key, nodesAsArray);
            }
        }

        /// <summary>
        /// Creates the Archive route - It's a fake page
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="nodeRoutePath"></param>
        /// <param name="nodesWithPath"></param>
        private static void MapNewsArchiveRoute(RouteCollection routes, string nodeRoutePath, IPublishedContent[] nodesWithPath)
        {
            foreach (var nodeSearch in nodesWithPath.GroupBy(x => x.GetPropertyValue<string>(PropertyAliases.NewsListingArchiveUrlName)))
            {
                if (string.IsNullOrWhiteSpace(nodeSearch.Key))
                    continue;

                var routeHash = nodeSearch.Key.GetHashCode();

                //Create the route for the /archive/{year}/{month}
                routes.MapUmbracoRoute(
                    string.Format(NewsArchiveRouteName, routeHash),
                    (nodeRoutePath.EnsureEndsWith('/') + nodeSearch.Key + "/{year}/{month}").TrimStart('/'),
                    new
                    {
                        controller = "NewsListing",
                        action = "NewsArchive",
                        year = UrlParameter.Optional,
                        month = UrlParameter.Optional
                    },
                    new NewsRouteHandler(nodesWithPath, nodeSearch.Key, nodeSearch.Key));
            }
        }

        private static void MapNewsTagsRoute(RouteCollection routes, string nodeRoutePath, IPublishedContent[] nodesWithPath)
        {
            foreach (var nodeSearch in nodesWithPath.GroupBy(x => x.GetPropertyValue<string>(PropertyAliases.NewsListingTagUrlName)))
            {
                if (string.IsNullOrWhiteSpace(nodeSearch.Key))
                    continue;

                var routeHash = nodeSearch.Key.GetHashCode();

                //Create the route for the /tag/{tag}/{group}
                routes.MapUmbracoRoute(
                    string.Format(NewsTagRouteName, routeHash),
                    (nodeRoutePath.EnsureEndsWith('/') + nodeSearch.Key + "/{tag}/{group}").TrimStart('/'),
                    new
                    {
                        controller = "NewsListing",
                        action = "NewsTagListing",
                        tag = UrlParameter.Optional,
                        group = UrlParameter.Optional
                    },
                    new NewsRouteHandler(nodesWithPath, nodeSearch.Key, nodeSearch.Key));
            }
        }

        /// <summary>
        /// Removes existing V8Media Framework custom routes
        /// </summary>
        /// <param name="routes"></param>
        private static void RemoveExisting(ICollection<RouteBase> routes)
        {
            var v8MediaRoutes = routes
                .OfType<Route>()
                .Where(x =>
                    x.DataTokens != null
                    && x.DataTokens.ContainsKey("__RouteName")
                    && ((string)x.DataTokens["__RouteName"]).InvariantStartsWith("v8media_"))
                .ToArray();

            foreach (var route in v8MediaRoutes)
            {
                routes.Remove(route);
            }
        }
    }
}