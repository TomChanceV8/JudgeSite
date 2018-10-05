using System;
using System.Text;
using System.Web.Mvc;
using Umbraco.Web.Models;
using V8Media.Framework.Domain;
using V8Media.Framework.Models.Form;
using V8Media.Framework.Models.PublishedContent;
using V8Media.Framework.Utilities.Extensions;

namespace V8Media.Framework.Controllers
{
    public class ContactController : SurfaceRenderMvcController
    {
        public ActionResult Contact(RenderModel model)
        {
            return CurrentTemplate(model);
        }

        [HttpPost]
        public ActionResult SendEmail(ContactFormModel contactFormModel)
        {
            if (!ModelState.IsValid)
            {
                ViewData[AppConstants.ViewDataSendEmailError] = "Send Email Failed";
                return CurrentUmbracoPage();
            }
            var contact = CurrentPage as Contact;

            if (contact == null)
            {
                throw new Exception("ContactController.SendEmail is only to be used with the a page which uses the Contact doctype");
            }

            var body = new StringBuilder();
            body.AppendLine("Name: " + contactFormModel.Name);
            body.AppendLine("Email: " + contactFormModel.Email);
            body.AppendLine("Comment: " + contactFormModel.Comment);

            Umbraco.SendEmail(contact.SenderEmailAddress, "", contact.RecipientEmailAddress, contact.Subject, body.ToString());

            return RedirectToUmbracoPage(contact.ThankyouPage);
        }
    }
}