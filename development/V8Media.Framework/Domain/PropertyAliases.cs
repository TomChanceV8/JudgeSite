namespace V8Media.Framework.Domain
{
    public class PropertyAliases
    {
        #region Umbraco Defaults

        public const string UmbracoNaviHide = "umbracoNaviHide";

        public const string UmbracoRedirect = "umbracoRedirect";

        public const string UmbracoUrlAlias = "umbracoUrlAlias";

        public const string UmbracoUrlName = "umbracoUrlName";

        public const string UmbracoInternalRedirectId = "umbracoInternalRedirectId";

        public const string UmbracoExamineScore = "examineScore";

        #endregion

        #region V8Media Framework Defaults

        public const string V8MediaHeader = "headerText";

        public const string V8MediaBodyText = "bodyText";

        public const string V8MediaExcerpt = "excerpt";

        public const string V8MediaAltTag = "altTag";
        #endregion

        #region DocType Website

        public const string WebsiteHeaderLinks = "headerLinks";

        public const string WebsiteFooterLinks = "footerLinks";

        public const string WebsiteGoogleAnalytics = "googleAnalytics";

        public const string WebsiteSitemapPriority = "sitemapPriority";

        public const string WebsiteSitemapChangeFreq = "sitemapFrequency";

        #endregion

        #region DocType Base

        public const string BaseMetaDescription = "metaDescription";

        public const string BaseMetaKeywords = "metaKeywords";

        public const string BasePageTitle = "pageTitle";

        public const string BaseOpenGraphTitle = "openGraphTitle";

        public const string BaseOpenGraphDescription = "openGraphDescription";

        public const string BaseOpenGraphImage = "openGraphImage";

        public const string BaseTwitterCardTitle = "twitterCardTitle";

        public const string BaseTwitterCardDescription = "twitterCardDescription";

        public const string BaseTwitterCardImage = "twitterCardImage";

        #endregion

        #region DocType Menu

        /// <summary>
        /// Menu title property alias
        /// DocType: Menu
        /// </summary>
        public const string MenuMenuTitle = "menuTitle";

        /// <summary>
        /// Menu Priority in sitemap property alias
        /// DocType: Menu
        /// </summary>
        public const string MenuSitemapPriority = "sitemapPriority";

        /// <summary>
        /// Menu Change Frequency in sitemap property alias
        /// DocType: Menu
        /// </summary>
        public const string MenuSitemapChangeFreq = "sitemapFrequency";
        #endregion

        #region DocType Home

        /// <summary>
        /// Home header property alias
        /// DocType: Home
        /// </summary>
        public const string HomeHeader = V8MediaHeader;

        #endregion

        #region DocType Textpage

        /// <summary>
        /// Textpage header property alias
        /// DocType: Textpage
        /// </summary>
        public const string TextpageHeader = V8MediaHeader;

        #endregion

        #region DocType Search

        /// <summary>
        /// Search header property alias
        /// DocType: Search
        /// </summary>
        public const string SearchHeader = V8MediaHeader;

        #endregion

        #region DocType NewsListing

        /// <summary>
        /// News listing header property alias
        /// DocType: NewsListing
        /// </summary>
        public const string NewsListingHeader = V8MediaHeader;

        /// <summary>
        /// The news listing archive URL name
        /// DocType: NewsListing
        /// </summary>
        public const string NewsListingArchiveUrlName = "archiveUrlName";

        /// <summary>
        /// The news listing tag URL name
        /// DocType: NewsListing
        /// </summary>
        public const string NewsListingTagUrlName = "tagUrlName";

        #endregion

        #region DocType News

        /// <summary>
        /// News header property alias
        /// DocType: News
        /// </summary>
        public const string NewsHeader = V8MediaHeader;

        /// <summary>
        /// News date property alias
        /// DocType: News
        /// </summary>
        public const string NewsNewsDate = "newsDate";

        /// <summary>
        /// News tags property alias
        /// DocType: News
        /// </summary>
        public const string NewsCategoriesTags = "newsCategoriesTags";

        #endregion

        #region DocType Contact

        /// <summary>
        /// Contact header property alias
        /// DocType: Contact
        /// </summary>
        public const string ContactHeader = V8MediaHeader;

        public const string ContactRecipientEmailAddress = "recipientEmailAddress";

        public const string ContactSenderEmailAddress = "senderEmailAddress";

        public const string ContactEmailSubject = "emailSubject";

        public const string ContactThankYouPage = "thankYouPage";

        #endregion
    }
}