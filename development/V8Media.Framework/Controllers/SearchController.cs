using System.Linq;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Models;
using V8Media.Framework.Domain;
using V8Media.Framework.Models.PublishedContent;
using V8Media.Framework.Utilities.Extensions;

namespace V8Media.Framework.Controllers
{
    public class SearchController : SurfaceRenderMvcController
    {
        public ActionResult Search(RenderModel model, string term)
        {
            var searchModel = new Search(model.Content);

            if (string.IsNullOrEmpty(term))
                return CurrentTemplate(searchModel);

            var searchResults =
                Umbraco.TypedSearch(term, true, AppConstants.SearchProviderWebsite).Where(x => x.IsVisible()).ToList();

            var pager = Umbraco.GetPager(Umbraco.GetDictionaryValueInt(AppConstants.DictionarySearchItemsPerPage, 10), searchResults.Count());

            searchModel.SearchResults = searchResults.Skip((pager.CurrentPage - 1) * pager.ItemsPerPage).Take(pager.ItemsPerPage).ToList();
            searchModel.Pager = pager;

            return CurrentTemplate(searchModel);
        }
    }
}