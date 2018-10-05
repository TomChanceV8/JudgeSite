namespace V8Media.Framework.Domain
{
    public class AppConstants
    {
        /// <summary>
        /// Umbraco Tag Group : News Categories
        /// </summary>
        public const string TagGroupNewsCategories = "NewsCategories";

        /// <summary>
        /// The creator name for a virtual page, enables us to identify a virtual page
        /// </summary>
        public const string VirtualPageCreatorName = "VirtualPageCreator";

        #region Examine

        /// <summary>
        /// The search provider for the website
        /// </summary>
        public const string SearchProviderWebsite = "WebsiteSearcher";

        /// <summary>
        /// The search provider for the extranet
        /// Supports protected content
        /// </summary>
        public const string SearchProviderExtranet = "ExtranetSearcher";

        /// <summary>
        /// The index provider for the website
        /// </summary>
        public const string IndexProviderWebsite = "WebsiteIndexer";

        /// <summary>
        /// The index provider for the extranet
        /// Supports protected content
        /// </summary>
        public const string IndexProviderExtranet = "ExtranetIndexer";

        #endregion

        #region ViewData/TempData/ViewBag Keys

        /// <summary>
        /// The view data pager next page key
        /// </summary>
        public const string ViewDataPagerNextPage = "PagerNextPage";

        /// <summary>
        /// The view data pager previous page key
        /// </summary>
        public const string ViewDataPagerPreviousPage = "PagerPreviousPage";

        /// <summary>
        /// The send email error view data key
        /// </summary>
        public const string ViewDataSendEmailError = "SendEmailError";

        #endregion

        #region Dictionary Keys

        /// <summary>
        /// The dictionary contact form subject key
        /// </summary>
        public const string DictionaryContactFormSubject = "ContactFormSubject";

        /// <summary>
        /// The dictionary news listing items per page key
        /// </summary>
        public const string DictionaryNewsListingItemsPerPage = "NewsListingItemsPerPage";

        /// <summary>
        /// The dictionary news archive items per page
        /// </summary>
        public const string DictionaryNewsArchiveItemsPerPage = "NewsArchiveItemsPerPage";

        /// <summary>
        /// The dictionary news tag items per page
        /// </summary>
        public const string DictionaryNewsTagItemsPerPage = "NewsTagItemsPerPage";

        /// <summary>
        /// The dictionary search items per page
        /// </summary>
        public const string DictionarySearchItemsPerPage = "SearchItemsPerPage";


        #endregion

        #region Querystring Keys

        /// <summary>
        /// The Pager page querystring
        /// </summary>
        public const string QuerystringPagerPage = "Page";

        #endregion
    }
}