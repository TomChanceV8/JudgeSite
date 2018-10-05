using System.Web.Mvc;

namespace V8Media.Framework.Controllers
{
    public class ScriptsAndStylesController : SurfaceRenderMvcController
    {
        /// <summary>
        /// Renders the base styles.
        /// Allows hild views to be cached using donut cache and still render the styles
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult RenderStyles()
        {
            return PartialView("~/Views/Partials/HtmlHead/BaseStyles.cshtml");
        }

        /// <summary>
        /// Renders the base scripts.
        /// Allows child views to be cached using donut cache and still render the scripts
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult RenderScripts()
        {
            return PartialView("~/Views/Partials/HtmlHead/BaseScripts.cshtml");
        }
    }
}
