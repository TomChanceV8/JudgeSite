using System.Web.Mvc;
using DevTrends.MvcDonutCaching;
using Umbraco.Web.Models;

namespace V8Media.Framework.Controllers
{
    public class DefaultController : BaseMenuController
    {
        /// <summary>
        /// If the route hijacking doesn't find a controller this default controller will be used.
        /// That way a each page will always go through a controller and we can always have a MasterModel for the masterpage.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [DonutOutputCache(CacheProfile = "OneDay")]
        public override ActionResult Index(RenderModel model)
        {
            return CurrentTemplate(model);
        }
    }
}