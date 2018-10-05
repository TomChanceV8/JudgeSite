using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Xml;

namespace V8Media.Framework.Mvc
{
    public class RssResult : ActionResult
    {
        private readonly SyndicationFeed _feed;

        public RssResult() { }

        public RssResult(SyndicationFeed feed)
        {
            _feed = feed;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/xml";

            var formatter = new Rss20FeedFormatter(_feed);

            using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.Output))
            {
                formatter.WriteTo(writer);
            }
        }
    }
}