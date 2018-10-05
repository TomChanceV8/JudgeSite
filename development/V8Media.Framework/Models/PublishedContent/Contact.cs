using Umbraco.Core.Configuration;
using Umbraco.Core.Models;
using Umbraco.Web;
using V8Media.Framework.Domain;
using V8Media.Framework.Utilities.Extensions;
using Zbu.ModelsBuilder;

namespace V8Media.Framework.Models.PublishedContent
{
    [RenamePropertyType(PropertyAliases.ContactHeader, "Header")]
    public partial class Contact
    {
        /// <summary>
        /// Gets the recipient email address.
        /// Fallback Website.Email->UmbracoSettings.confid:NotificationEmailAddress
        /// </summary>
        /// <value>
        /// The recipient email address.
        /// </value>
        [ImplementPropertyType(PropertyAliases.ContactRecipientEmailAddress)]
        public string RecipientEmailAddress
        {
            get
            {
                return Umbraco.Coalesce(this.GetPropertyValue<string>(PropertyAliases.ContactRecipientEmailAddress),
                    this.Website().Email, UmbracoConfig.For.UmbracoSettings().Content.NotificationEmailAddress);
            }
        }

        /// <summary>
        /// Gets the sender email address.
        /// Fallback Website.Email->UmbracoSettings.confid:NotificationEmailAddress
        /// </summary>
        /// <value>
        /// The sender email address.
        /// </value>
        [ImplementPropertyType(PropertyAliases.ContactSenderEmailAddress)]
        public string SenderEmailAddress
        {
            get
            {
                return Umbraco.Coalesce(this.GetPropertyValue<string>(PropertyAliases.ContactSenderEmailAddress),
                    this.Website().Email, UmbracoConfig.For.UmbracoSettings().Content.NotificationEmailAddress);
            }
        }

        /// <summary>
        /// Gets the subject.
        /// Fallbacks to the dictionary item V8Media->Defaults->ContactFormSubject
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        [ImplementPropertyType(PropertyAliases.ContactEmailSubject)]
        public string Subject
        {
            get
            {
                return Umbraco.Coalesce(this.GetPropertyValue<string>(PropertyAliases.ContactEmailSubject),
                    Umbraco.GetDictionaryValue(AppConstants.DictionaryContactFormSubject));
            }
        }

        /// <summary>
        /// Gets the thankyou page.
        /// Fallbacks to contact page if not set
        /// </summary>
        /// <value>
        /// The thankyou page.
        /// </value>
        [ImplementPropertyType(PropertyAliases.ContactThankYouPage)]
        public IPublishedContent ThankyouPage
        {
            get
            {
                return this.GetPropertyValue<IPublishedContent>(PropertyAliases.ContactThankYouPage) ?? this;
            }
        }
    }
}