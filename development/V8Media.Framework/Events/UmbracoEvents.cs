using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;
using DevTrends.MvcDonutCaching;
using Examine;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Publishing;
using Umbraco.Core.Services;
using Umbraco.Core.Sync;
using Umbraco.Web;
using Umbraco.Web.Cache;
using Umbraco.Web.Mvc;
using Umbraco.Web.Routing;
using V8Media.Framework.Controllers;
using V8Media.Framework.Domain;
using V8Media.Framework.Routes;

namespace V8Media.Framework.Events
{
    public class UmbracoEvents : ApplicationEventHandler
    {
        protected static UmbracoHelper Umbraco
        {
            get { return new UmbracoHelper(UmbracoContext.Current); }
        }

        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication,
            ApplicationContext applicationContext)
        {
            ContentService.Saved += ContentService_Saved;
            ContentService.Published += Content_Published;
            ContentService.UnPublished += Content_Unpublished;
            ContentService.Moved += Content_Moved;
            ContentService.Trashed += Content_Trashed;
            ContentService.Deleted += Content_Deleted;
            MediaService.Saved += Media_Saved;

            UrlProviderResolver.Current.AddType<VirtualNodeUrlProvider>();

            // TODO: Comment if you want to disable the default controller 
            //By registering this here we can make sure that if route hijacking doesn't find a controller it will use this controller.
            DefaultRenderMvcControllerResolver.Current.SetDefaultControllerType(typeof(DefaultController));

            var types = PluginManager.Current.ResolveTypes<PublishedContentModel>();
            var factory = new PublishedContentModelFactory(types);
            PublishedContentModelFactoryResolver.Current.SetFactory(factory);
        }

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            //list to the init event of the application base, this allows us to bind to the actual HttpApplication events
            UmbracoApplicationBase.ApplicationInit += UmbracoApplicationBase_ApplicationInit;

            // Map custom routes
            //RouteMapper.MapRoutes(RouteTable.Routes, UmbracoContext.Current.ContentCache);

            // Umbraco event subsriptions
            ExamineManager.Instance.IndexProviderCollection[AppConstants.IndexProviderWebsite].GatheringNodeData += OnGatheringNodeData;
            ExamineManager.Instance.IndexProviderCollection[AppConstants.IndexProviderExtranet].GatheringNodeData +=
                OnGatheringNodeData;

            PageCacheRefresher.CacheUpdated += PageCacheRefresher_CacheUpdated;

            //PreRenderViewActionFilterAttribute.ActionExecuted += PreRenderViewActionFilterAttribute_ActionExecuted;
        }

        /// <summary>
        /// When the page cache is refreshed, we'll check if any news root nodes were included in the refresh, if so we'll set a flag
        /// on the current request to rebuild the routes at the end of the request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// This will also work for load balanced scenarios since this event executes on all servers
        /// </remarks>
        private void PageCacheRefresher_CacheUpdated(PageCacheRefresher sender, Umbraco.Core.Cache.CacheRefresherEventArgs e)
        {
            if (UmbracoContext.Current == null) return;

            switch (e.MessageType)
            {
                case MessageType.RefreshById:
                case MessageType.RemoveById:
                    var item = UmbracoContext.Current.ContentCache.GetById((int)e.MessageObject);
                    if (item != null && item.DocumentTypeAlias.InvariantEquals(DocTypeAliases.NewsListing))
                    {
                        //add the unpublished entities to the request cache
                        ApplicationContext.Current.ApplicationCache.RequestCache.GetCacheItem("v8media-refresh-routes", () => true);
                    }
                    break;
                case MessageType.RefreshByInstance:
                case MessageType.RemoveByInstance:
                    var content = e.MessageObject as IContent;
                    if (content == null) return;
                    if (content.ContentType.Alias.InvariantEquals(DocTypeAliases.NewsListing))
                    {
                        //add the unpublished entities to the request cache
                        UmbracoContext.Current.Application.ApplicationCache.RequestCache.GetCacheItem("v8media-refresh-routes", () => true);
                    }
                    break;
            }
        }

        /// <summary>
        /// Bind to the PostRequestHandlerExecute event of the HttpApplication
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UmbracoApplicationBase_ApplicationInit(object sender, EventArgs e)
        {
            var app = (UmbracoApplicationBase)sender;
            app.PostRequestHandlerExecute += UmbracoApplication_PostRequestHandlerExecute;
        }

        /// <summary>
        /// When the page cache is refreshed, we'll check if any news root nodes were included in the refresh, if so we'll set a flag
        /// on the current request to rebuild the routes at the end of the request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// This will also work for load balanced scenarios since this event executes on all servers
        /// </remarks>
        private void UmbracoApplication_PostRequestHandlerExecute(object sender, EventArgs e)
        {
            if (ApplicationContext.Current == null) return;
            if (ApplicationContext.Current.ApplicationCache.RequestCache.GetCacheItem("v8media-refresh-routes") == null) return;
            //the token was found so that means one or more news root nodes were changed in this request, rebuild the routes.
           // RouteMapper.MapRoutes(RouteTable.Routes, UmbracoContext.Current.ContentCache);
        }

        private void ContentService_Saved(IContentService sender, SaveEventArgs<IContent> e)
        {
            ClearCache();
        }

        private void Content_Published(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            ClearCache();
        }

        private void Content_Deleted(IContentService sender, DeleteEventArgs<IContent> e)
        {
            ClearCache();
        }

        private void Content_Trashed(IContentService sender, MoveEventArgs<IContent> e)
        {
            ClearCache();
        }

        private void Content_Moved(IContentService sender, MoveEventArgs<IContent> e)
        {
            ClearCache();
        }

        private void Content_Unpublished(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            ClearCache();
        }

        private void Media_Saved(IMediaService sender, SaveEventArgs<IMedia> e)
        {
            ClearCache();
        }

        private void OnGatheringNodeData(object sender, IndexingNodeDataEventArgs e)
        {
            // Create searchable path
            if (e.Fields.ContainsKey("path"))
            {
                e.Fields["searchPath"] = e.Fields["path"].Replace(',', ' ');
            }

            // Lowercase all the fields for case insensitive searching
            var keys = e.Fields.Keys.ToList();
            foreach (var key in keys)
            {
                e.Fields[key] = HttpUtility.HtmlDecode(e.Fields[key].ToLower(CultureInfo.InvariantCulture));
            }

            // Stuff all the fields into a single field for easier searching
            var combinedFields = new StringBuilder();
            foreach (var keyValuePair in e.Fields)
            {
                combinedFields.AppendLine(keyValuePair.Value);
            }
            e.Fields.Add("contents", combinedFields.ToString());
        }

        protected void PreRenderViewActionFilterAttribute_ActionExecuted(object sender, ActionExecutedEventArgs e)
        {
            //In this event it's possible to modify the model that will go the view. 
            //If we use a package that has it's own route hijacking (for example Articulate) we can still give it our own master model here.
        }

        private void ClearCache()
        {
            try
            {
                //Clear all output cache so everything is refreshed.
                var cacheManager = new OutputCacheManager();
                cacheManager.RemoveItems();

                //Clear any cache in the HttpContext Cache
                //HttpContext.Current.Cache.Remove("CachedNewsNodes");
            }
            catch (Exception ex)
            {
                LogHelper.Error<UmbracoEvents>(string.Format("Exception: {0} - StackTrace: {1}", ex.Message, ex.StackTrace), ex);
            }
        }
    }
}