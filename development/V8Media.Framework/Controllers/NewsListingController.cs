using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DevTrends.MvcDonutCaching;
using Umbraco.Web.Models;
using V8Media.Framework.Domain;
using V8Media.Framework.Models.PublishedContent;
using V8Media.Framework.Services;
using V8Media.Framework.Utilities.Extensions;

namespace V8Media.Framework.Controllers
{
    public class NewsListingController : BaseMenuController
    {
        [DonutOutputCache(CacheProfile = "OneDay")]
        public ActionResult NewsListing(RenderModel model)
        {
            var newsListingModel = new NewsListing(model.Content);

            var newsItems = ServiceFactory.GetService<NewsService>().GetLatestNews(CurrentPage).ToList();
            var pager = Umbraco.GetPager(Umbraco.GetDictionaryValueInt(AppConstants.DictionaryNewsListingItemsPerPage, 10), newsItems.Count());

            newsListingModel.NewsArticles = newsItems.Skip((pager.CurrentPage - 1) * pager.ItemsPerPage).Take(pager.ItemsPerPage).ToList();
            newsListingModel.Pager = pager;
            
            return CurrentTemplate(newsListingModel);
        }

        [DonutOutputCache(CacheProfile = "OneDay")]
        public ActionResult NewsArchive(RenderModel model, string year, string month, int? p = null)
        {
            var newsListingModel = new NewsListing(model.Content);

            int intYear;
            var newsItems = new List<NewsArticle>();
            if (!string.IsNullOrEmpty(year) && year.Length == 4 && int.TryParse(year, out intYear))
            {
                newsListingModel.FilteredArchiveYear = new DateTime(intYear, DateTime.Now.Month, DateTime.Now.Day);
                int intMonth;
                if (!string.IsNullOrEmpty(month) && month.Length == 2 && int.TryParse(month, out intMonth))
                {
                    var monthDateTime = new DateTime(newsListingModel.FilteredArchiveYear.Value.Year, intMonth, DateTime.Now.Day);
                    newsItems = ServiceFactory.GetService<NewsService>().GetFilteredNews(newsListingModel, newsListingModel.FilteredArchiveYear.Value, monthDateTime).ToList();
                    newsListingModel.FilteredArchiveMonth = monthDateTime;
                }
                else
                {
                    newsItems = ServiceFactory.GetService<NewsService>().GetFilteredNews(newsListingModel, newsListingModel.FilteredArchiveYear.Value).ToList();
                }
            }

            var pager = Umbraco.GetPager(Umbraco.GetDictionaryValueInt(AppConstants.DictionaryNewsArchiveItemsPerPage, 10), newsItems.Count());
            newsListingModel.NewsArticles = newsItems.Skip((pager.CurrentPage - 1) * pager.ItemsPerPage).Take(pager.ItemsPerPage).ToList();
            newsListingModel.Pager = pager;
            return CurrentTemplate(newsListingModel);
        }

        [DonutOutputCache(CacheProfile = "OneDay")]
        public ActionResult NewsTagListing(RenderModel model, string tag, string group, int? p = null)
        {
            var newsListingModel = new NewsListing(model.Content);
            var newsItems = new List<NewsArticle>();
            if (!string.IsNullOrEmpty(tag))
            {
                newsItems = ServiceFactory.GetService<NewsService>().GetFilteredNews(newsListingModel, tag, !string.IsNullOrEmpty(@group) ? @group : AppConstants.TagGroupNewsCategories).ToList();
                newsListingModel.FilteredTag = tag;
            }
            var pager = Umbraco.GetPager(Umbraco.GetDictionaryValueInt(AppConstants.DictionaryNewsTagItemsPerPage, 10), newsItems.Count());
            newsListingModel.NewsArticles = newsItems.Skip((pager.CurrentPage - 1) * pager.ItemsPerPage).Take(pager.ItemsPerPage).ToList();
            newsListingModel.Pager = pager;
            return CurrentTemplate(newsListingModel);
        }
    }
}