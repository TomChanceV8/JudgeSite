using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Archetype.Models;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using V8Media.Framework.Domain;
using V8Media.Framework.Models.PublishedContent;
using V8Media.Framework.Models.ViewModels;

namespace V8Media.Framework.Utilities.Extensions
{
    public static class PublishedContentExtensions
    {
        private static UmbracoHelper Umbraco
        {
            get { return new UmbracoHelper(UmbracoContext.Current); }
        }

        /// <summary>
        /// Return the Website node where default settings are stored as IPublishedContent.
        /// </summary>
        public static Website Website(this IPublishedContent content, bool noCache = false)
        {
            var website = (IPublishedContent)HttpContext.Current.Items["Website"];

            if (website == null || noCache)
            {
                website = Umbraco.TypedContentAtRoot().First(c => c.DocumentTypeAlias == DocTypeAliases.Website);

                if (!noCache)
                {
                    HttpContext.Current.Items["Website"] = website;
                }
            }

            return new Website(website);
        }

        /// <summary>
        /// Gets the menu items for passed in content
        /// </summary>
        /// <param name="currentContent">Content of the current.</param>
        /// <param name="childDepth"></param>
        /// <returns></returns>
        public static IEnumerable<MenuItem> GetMenuItems(this IPublishedContent currentContent, int childDepth = 0)
        {

            return
                (
                    from n in currentContent.Children.OfType<Menu>()
                    where !n.HideInMenu
                    select new MenuItem
                    {
                        Id = n.Id,
                        Title = n.MenuTitle,
                        Url = n.Url,
                        ActiveClass = currentContent.Path.Contains(n.Id.ToString(CultureInfo.InvariantCulture)) ? "active" : null,
                        ChildMenuItems = (childDepth <= 0 ? new List<MenuItem>() : AddChildrenMenuItems(n, 0, childDepth))
                    }
                );
        }

        /// <summary>
        /// Gets the menu items based on a multi url picker.
        /// </summary>
        /// <param name="currentContent">Content of the current.</param>
        /// <param name="pickedNavigation">The picked navigation.</param>
        /// <returns></returns>
        public static IEnumerable<MenuItem> GetMenuItems(this IPublishedContent currentContent,
            IEnumerable<UrlPicker.Umbraco.Models.UrlPicker> pickedNavigation)
        {
            return
                (
                    from n in pickedNavigation
                    select new MenuItem
                    {
                        Id = n.TypeData.ContentId.HasValue
                                ? -1
                                : n.TypeData.ContentId.GetValueOrDefault(),
                        Title = n.Meta.Title,
                        Url = n.Url,
                        ActiveClass = n.TypeData.ContentId.HasValue
                                ? (currentContent.Path.Contains(
                                    n.TypeData.ContentId.Value.ToString(CultureInfo.InvariantCulture))
                                    ? "active"
                                    : null)
                                : null
                    }
                );
        }

        private static IEnumerable<MenuItem> AddChildrenMenuItems(IPublishedContent content, int currentDepth, int childDepth)
        {
            currentDepth++;
            return
            (
                from n in content.Children.OfType<Menu>()
                where !n.HideInMenu
                select new MenuItem
                {
                    Id = n.Id,
                    Title = n.MenuTitle,
                    Url = n.Url,
                    ActiveClass = content.Path.Contains(n.Id.ToString(CultureInfo.InvariantCulture)) ? "active" : null,
                    ChildMenuItems = (currentDepth >= childDepth ? new List<MenuItem>() : AddChildrenMenuItems(n, currentDepth, childDepth))
                }
            );
        }

        /// <summary>
        /// Checks if the model has a property and a value for the property
        /// </summary>
        /// <param name="model">
        /// The <see cref="IPublishedContent"/> to inspect
        /// </param>
        /// <param name="propertyAlias">
        /// The Umbraco property alias on the <see cref="IPublishedContent"/>
        /// </param>
        /// <returns>
        /// A value indicating whether or not the property exists on the <see cref="IPublishedContent"/> and has a value
        /// </returns>
        public static bool WillWork(this IPublishedContent model, string propertyAlias)
        {
            return model.HasProperty(propertyAlias) && model.HasValue(propertyAlias);
        }

        /// <summary>
        /// Gets strongly typed multi url picker
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="propertyAlias">The property alias.</param>
        /// <returns></returns>
        public static IEnumerable<UrlPicker.Umbraco.Models.UrlPicker> MultiLUrlPicker(this IPublishedContent content, string propertyAlias)
        {
            var archetypeModel = content.Website().GetPropertyValue<ArchetypeModel>(propertyAlias);
            return archetypeModel.Select(x => x.GetValue<UrlPicker.Umbraco.Models.UrlPicker>("urlPicker")).ToList();
        }

        /// <summary>
        /// Get the examines the score.
        /// Is only set when got via TypedSearch Or Search
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public static decimal ExamineScore(this IPublishedContent content)
        {
            if (content.GetProperty(PropertyAliases.UmbracoExamineScore) != null)
            {
                return decimal.Parse(content.GetProperty(PropertyAliases.UmbracoExamineScore).Value.ToString());
            }
            return 0;
        }

        /// <summary>
        /// Determines whether IPublishedContent is a virtual node or a real umbraco node
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public static bool IsVirtualPage(this IPublishedContent content)
        {
            return content.CreatorName.InvariantEquals(AppConstants.VirtualPageCreatorName);
        }
    }
}