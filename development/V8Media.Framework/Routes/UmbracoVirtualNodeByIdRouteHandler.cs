﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace V8Media.Framework.Routes
{
    public class UmbracoVirtualNodeByIdRouteHandler : UmbracoVirtualNodeRouteHandler
    {
        private readonly List<Tuple<string, int>> _hostsAndIds = new List<Tuple<string, int>>();

        /// <summary>
        /// Constructor used to create a new handler for multi-tenency with domains and ids
        /// </summary>
        /// <param name="itemsForRoute"></param>
        public UmbracoVirtualNodeByIdRouteHandler(IEnumerable<IPublishedContent> itemsForRoute)
        {
            foreach (var publishedContent in itemsForRoute)
            {
                var url = publishedContent.Url;
                //if there is a double slash, it will have a domain
                if (url.Contains("//"))
                {
                    var uri = new Uri(url, UriKind.Absolute);
                    _hostsAndIds.Add(new Tuple<string, int>(uri.Host, publishedContent.Id));
                }
                else
                {
                    _hostsAndIds.Add(new Tuple<string, int>(string.Empty, publishedContent.Id));
                }
            }
        }

        /// <summary>
        /// Constructor used to create a new handler for only one id and no domain
        /// </summary>
        /// <param name="realNodeId"></param>
        public UmbracoVirtualNodeByIdRouteHandler(int realNodeId)
        {
            _hostsAndIds.Add(new Tuple<string, int>(string.Empty, realNodeId));
        }


        protected sealed override IPublishedContent FindContent(RequestContext requestContext, UmbracoContext umbracoContext)
        {
            //determine if it's for a particular domain
            int realNodeId;
            if (_hostsAndIds.Count == 1)
            {
                realNodeId = _hostsAndIds.First().Item2;
            }
            else
            {
                realNodeId = requestContext.HttpContext.Request.Url == null
                    ? _hostsAndIds.First().Item2 //cannot be determined
                    : requestContext.HttpContext.Request.Url.Host.InvariantEquals("localhost")
                        ? _hostsAndIds.First(x => x.Item1 == string.Empty).Item2
                        : _hostsAndIds.First(x => x.Item1.InvariantEquals(requestContext.HttpContext.Request.Url.Host)).Item2;
            }

            var byId = umbracoContext.ContentCache.GetById(realNodeId);
            if (byId == null) return null;

            return FindContent(requestContext, umbracoContext, byId);
        }

        protected virtual IPublishedContent FindContent(RequestContext requestContext, UmbracoContext umbracoContext, IPublishedContent baseContent)
        {
            return baseContent;
        }
    }
}