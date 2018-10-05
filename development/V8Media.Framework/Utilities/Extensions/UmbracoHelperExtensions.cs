using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using Umbraco.Core.Logging;
using Umbraco.Web;
using V8Media.Framework.Domain;
using V8Media.Framework.Models.Custom;

namespace V8Media.Framework.Utilities.Extensions
{
    public static class UmbracoHelperExtensions
    {
        #region Error

        /// <summary>
        /// Log an exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="umbraco">The umbraco.</param>
        /// <param name="ex">The ex.</param>
        public static void LogException<T>(this UmbracoHelper umbraco, Exception ex)
        {
            try
            {
                var nodeId = -1;
                if (HttpContext.Current.Items["pageID"] != null)
                {
                    int.TryParse(HttpContext.Current.Items["pageID"].ToString(), out nodeId);
                }

                var comment = new StringBuilder();
                var commentHtml = new StringBuilder();

                commentHtml.AppendFormat("<p><strong>Url:</strong><br/>{0}</p>", HttpContext.Current.Request.Url.AbsoluteUri);
                commentHtml.AppendFormat("<p><strong>Node id:</strong><br/>{0}</p>", nodeId);

                //Add the exception.
                comment.AppendFormat("Exception: {0} - StackTrace: {1}", ex.Message, ex.StackTrace);
                commentHtml.AppendFormat("<p><strong>Exception:</strong><br/>{0}</p>", ex.Message);
                commentHtml.AppendFormat("<p><strong>StackTrace:</strong><br/>{0}</p>", ex.StackTrace);

                if (ex.InnerException != null)
                {
                    //Add the inner exception.
                    comment.AppendFormat("- InnerException: {0} - InnerStackTrace: {1}", ex.InnerException.Message, ex.InnerException.StackTrace);
                    commentHtml.AppendFormat("<p><strong>InnerException:</strong><br/>{0}</p>", ex.InnerException.Message);
                    commentHtml.AppendFormat("<p><strong>InnerStackTrace:</strong><br/>{0}</p>", ex.InnerException.StackTrace);
                }

                //Log the Exception into Umbraco.
                LogHelper.Error<T>(comment.ToString(), ex);
            }
            catch
            {
                //Do nothing because nothing can be done with this exception.
            }
        }

        #endregion

        #region Email

        /// <summary>
        /// Send the e-mail.
        /// </summary>
        /// <param name="umbraco">The umbraco.</param>
        /// <param name="emailFrom">The email from.</param>
        /// <param name="emailFromName">Name of the email from.</param>
        /// <param name="emailTo">The email to.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="emailCc">The email cc.</param>
        /// <param name="emailBcc">The email BCC.</param>
        /// <param name="addFiles">if set to <c>true</c> [add files].</param>
        /// <param name="attachments">The attachments.</param>
        public static void SendEmail(
            this UmbracoHelper umbraco,
            string emailFrom,
            string emailFromName,
            string emailTo,
            string subject,
            string body,
            string emailCc = "",
            string emailBcc = "",
            bool addFiles = false,
            Dictionary<string, MemoryStream> attachments = null
            )
        {
            //Make the MailMessage and set the e-mail address.
            var message = new MailMessage {From = new MailAddress(emailFrom, emailFromName)};

            //Split all the e-mail addresses on , or ;.
            char[] splitChar = { ',', ';' };

            //Remove all spaces.
            emailTo = emailTo.Trim();
            emailCc = emailCc.Trim();
            emailBcc = emailBcc.Trim();

            //Split emailTo.
            var toArray = emailTo.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
            foreach (var to in toArray.Where(to => to.IsValidEmail()))
            {
                //Loop through all e-mail addressen in toArray and add them in the to.
                message.To.Add(new MailAddress(to));
            }

            //Split emailCc.
            var ccArray = emailCc.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
            foreach (var cc in ccArray.Where(cc => cc.IsValidEmail()))
            {
                //Loop through all e-mail addressen in ccArray and add them in the cc.
                message.CC.Add(new MailAddress(cc));
            }

            //Split emailBcc.
            var bccArray = emailBcc.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
            foreach (var bcc in bccArray.Where(bcc => bcc.IsValidEmail()))
            {
                //Loop through all e-mail addressen in bccArray and add them in the bcc.
                message.Bcc.Add(new MailAddress(bcc));
            }

            if (addFiles && attachments != null)
            {
                foreach (var att in attachments.Select(attachment => new Attachment(attachment.Value, attachment.Key)))
                {
                    message.Attachments.Add(att);
                }
            }

            //Set the rest of the e-mail data.
            message.Subject = subject;
            message.SubjectEncoding = Encoding.UTF8;
            message.Body = body;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;

            //Only send the email if there is a to address.
            if (message.To.Any())
            {
                //Make the SmtpClient.
                var smtpClient = new SmtpClient();

                //Send the e-mail.
                smtpClient.Send(message);
            }

            //Clear the resources.
            message.Dispose();
        }

        #endregion

        #region Form

        /// <summary>
        /// Replaces the placeholders expects placeholders to be wrapped square brackets.
        /// </summary>
        /// <param name="templateString">The template string.</param>
        /// <param name="phData">The ph data.</param>
        /// <param name="escapeHtml">if set to <c>true</c> [escape HTML].</param>
        /// <returns></returns>
        private static string ReplacePlaceholders(string templateString, Dictionary<string, string> phData, bool escapeHtml = false)
        {
            var templ = new StringBuilder(templateString);

            foreach (var kv in phData)
            {
                var val = kv.Value;
                if (escapeHtml)
                {
                    val = HttpContext.Current.Server.HtmlEncode(val);
                }
                templ.Replace("[" + kv.Key + "]", val);
            }

            return templ.ToString();
        }

        #endregion

        #region Pager

        /// <summary>
        /// Return all fields required for paging.
        /// </summary>
        /// <param name="umbraco">The umbraco.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="numberOfItems">The number of items.</param>
        /// <returns></returns>
        public static Pager GetPager(this UmbracoHelper umbraco, int itemsPerPage, int numberOfItems)
        {
            // paging calculations
            int currentPage = int.TryParse(HttpContext.Current.Request.QueryString[AppConstants.QuerystringPagerPage], out currentPage) ? currentPage : 1;
            var numberOfPages = numberOfItems % itemsPerPage == 0 ? Math.Ceiling((decimal)(numberOfItems / itemsPerPage)) : Math.Ceiling((decimal)(numberOfItems / itemsPerPage)) + 1;
            var pages = Enumerable.Range(1, (int)numberOfPages).ToList();

            return new Pager
            {
                NumberOfItems = numberOfItems,
                ItemsPerPage = itemsPerPage,
                CurrentPage = currentPage,
                Pages = pages
            };
        }

        #endregion

        #region Url

        /// <summary>
        /// Appends or updates a query string value to the current Url
        /// </summary>
        /// <param name="umbraco"></param>
        /// <param name="key">The query string key</param>
        /// <param name="value">The query string value</param>
        /// <returns>The updated Url</returns>
        public static string AppendOrUpdateQueryString(this UmbracoHelper umbraco, string key, string value)
        {
            return umbraco.AppendOrUpdateQueryString(HttpContext.Current.Request.RawUrl, key, value);
        }

        /// <summary>
        /// Appends or updates a query string value to supplied Url
        /// </summary>
        /// <param name="umbraco"></param>
        /// <param name="url">The Url to update</param>
        /// <param name="key">The query string key</param>
        /// <param name="value">The query string value</param>
        /// <returns>The updated Url</returns>
        public static string AppendOrUpdateQueryString(this UmbracoHelper umbraco, string url, string key, string value)
        {
            const char q = '?';

            if (url.IndexOf(q) == -1)
            {
                return string.Concat(url, q, key, '=', HttpUtility.UrlEncode(value));
            }

            var baseUrl = url.Substring(0, url.IndexOf(q));
            var queryString = url.Substring(url.IndexOf(q) + 1);
            var match = false;
            var kvps = HttpUtility.ParseQueryString(queryString);

            foreach (var queryStringKey in kvps.AllKeys)
            {
                if (queryStringKey == key)
                {
                    kvps[queryStringKey] = value;
                    match = true;
                    break;
                }
            }

            if (!match)
            {
                kvps.Add(key, value);
            }

            return string.Concat(baseUrl, q, ConstructQueryString(kvps, null, false));
        }

        /// <summary>
        /// Constructs a NameValueCollection into a query string.
        /// </summary>
        /// <remarks>Consider this method to be the opposite of "System.Web.HttpUtility.ParseQueryString"</remarks>
        /// <param name="parameters">The NameValueCollection</param>
        /// <param name="delimiter">The String to delimit the key/value pairs</param>
        /// <param name="omitEmpty">Boolean to chose whether to omit empty values</param>
        /// <returns>A key/value structured query string, delimited by the specified String</returns>
        /// <example>
        /// http://blog.leekelleher.com/2009/09/19/how-to-convert-namevaluecollection-to-a-query-string-revised/
        /// </example>
        private static string ConstructQueryString(NameValueCollection parameters, string delimiter, bool omitEmpty)
        {
            if (string.IsNullOrEmpty(delimiter))
                delimiter = "&";

            const char @equals = '=';
            var items = new List<string>();

            for (var i = 0; i < parameters.Count; i++)
            {
                foreach (var value in parameters.GetValues(i))
                {
                    var addValue = !omitEmpty || !string.IsNullOrEmpty(value);
                    if (addValue)
                    {
                        items.Add(string.Concat(parameters.GetKey(i), equals, HttpUtility.UrlEncode(value)));
                    }
                }
            }

            return string.Join(delimiter, items.ToArray());
        }
        #endregion

        #region Dictionary

        public static int GetDictionaryValueInt(this UmbracoHelper umbraco, string dictionaryKey, int defualt)
        {
            int value;
            return int.TryParse(umbraco.GetDictionaryValue(dictionaryKey), out value) ? value : defualt;
        }

        public static string GetDictionaryValueString(this UmbracoHelper umbraco, string dictionaryKey, string defualt)
        {
            var value = umbraco.GetDictionaryValue(dictionaryKey);
            return string.IsNullOrEmpty(value) ? defualt : value;
        }

        #endregion
    }
}