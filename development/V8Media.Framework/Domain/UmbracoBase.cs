using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;
using Umbraco.Web;

namespace V8Media.Framework.Domain
{
    public class UmbracoBase
    {
        protected static Database Database
        {
            get { return ApplicationContext.Current.DatabaseContext.Database; }
        }

        protected static ServiceContext Services
        {
            get { return ApplicationContext.Current.Services; }
        }

        protected static IPublishedContent CurrentPage
        {
            get { return UmbracoContext.Current.PublishedContentRequest.PublishedContent; }
        }

        protected static UmbracoHelper Umbraco
        {
            get { return new UmbracoHelper(UmbracoContext.Current); }
        }
    }
}