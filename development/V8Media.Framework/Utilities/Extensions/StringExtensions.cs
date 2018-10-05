using System;
using System.Linq;
using System.Web;
using Umbraco.Core;

namespace V8Media.Framework.Utilities.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// This tries to detect a json string, this is not a fail safe way but it is quicker than doing 
        /// a try/catch when deserializing when it is not json.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool DetectIsJson(this string input)
        {
            input = input.Trim();
            return (input.StartsWith("{") && input.EndsWith("}"))
                   || (input.StartsWith("[") && input.EndsWith("]"));
        }

        /// <summary>
        /// Checks if the e-mail is valid.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(this string email)
        {
            return RegexHelpers.Email.IsMatch(email);
        }

        public static string SafeEncodeUrlSegments(this string urlPath)
        {
            return string.Join("/",
                urlPath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => HttpUtility.UrlEncode(x).Replace("+", "%20"))
                    .WhereNotNull()
                //we are not supporting dots in our URLs it's just too difficult to
                // support across the board with all the different config options
                    .Select(x => x.Replace('.', '-')));
        }

        /// <summary>
        /// Gets the absolute absolut url.
        /// </summary>
        /// <param name="relativeUrl">
        /// The relative url.
        /// </param>
        /// <returns>
        /// The absolute url.
        /// </returns>
        /// <exception cref="NullReferenceException">
        /// Throws a null reference exception if the HttpContext is null.
        /// </exception>
        public static string MakeAbsolutUrl(this string relativeUrl)
        {
            return relativeUrl.MakeAbsolutUrl(string.Empty);
        } 

        /// <summary>
        /// Gets the absolute absolut url.
        /// </summary>
        /// <param name="relativeUrl">
        /// The relative url.
        /// </param>
        /// <param name="queryString">
        /// The query string.
        /// </param>
        /// <returns>
        /// The absolute url.
        /// </returns>
        /// <exception cref="NullReferenceException">
        /// Throws a null reference exception if the HttpContext is null.
        /// </exception>
        public static string MakeAbsolutUrl(this string relativeUrl, string queryString)
        {
            if (relativeUrl.StartsWith("http")) return relativeUrl;

            var context = HttpContext.Current;
            if (context == null) throw new NullReferenceException("The HttpContext is null");

            var protocol = context.Request.IsSecureConnection ? "https://" : "http://";
            var host = context.Request.ServerVariables["SERVER_NAME"];
            var port = context.Request.ServerVariables["SERVER_PORT"];
            port = port == "80" ? string.Empty : string.Format(":{0}", port);

            if (!relativeUrl.StartsWith("/")) relativeUrl = string.Format("/{0}", relativeUrl);
            if (!queryString.StartsWith("?") && !string.IsNullOrEmpty(queryString)) queryString = string.Format("?{0}", queryString);

            return string.Format("{0}{1}{2}{3}{4}", protocol, host, port, relativeUrl, queryString);
        }

        /// <summary>
        /// Add an absolute path to all umbraco image hrefs in html string.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="uri">The absolute URI.</param>
        /// <returns></returns>
        public static string UmbracoMediaHrefToAbsoluteUri(this string html, string uri)
        {
            if (string.IsNullOrEmpty(html) || string.IsNullOrEmpty(uri))
                return null;
            return RegexHelpers.UmbracoMediaHref.Replace(html, match =>
            {
                if (match.Groups.Count == 2)
                {
                    return " href=\"" +
                           uri.TrimEnd('/') + match.Groups[1].Value.EnsureStartsWith('/') +
                           "\"";
                }
                return null;
            });
        }

        /// <summary>
        /// Add an absolute path to all umbraco image src in html.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="uri">The absolute URI.</param>
        /// <returns></returns>
        public static string UmbracoMediaSrcToAbsoluteUri(this string html, string uri)
        {
            if (string.IsNullOrEmpty(html) || string.IsNullOrEmpty(uri))
                return null;
            return RegexHelpers.UmbracoMediaSrc.Replace(html, match =>
            {
                if (match.Groups.Count == 2)
                {
                    return " src=\"" +
                           uri.TrimEnd('/') + match.Groups[1].Value.EnsureStartsWith('/') +
                           "\"";
                }
                return null;
            });
        }
    }
}